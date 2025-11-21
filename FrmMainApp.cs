using CsvHelper;
using HLWebScraper.Net.Helpers;
using HLWebScraper.Net.Model;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

#pragma warning disable CA1416

namespace HLWebScraper.Net;

public partial class FrmMainApp : Form
{
    private const string FxUrl = "https://www.floatrates.com/daily/gbp.json";
    private const string Not_ETF_ETFType = "## Not ETF";
    internal static readonly FXCurrencyCollection FxCurrencies = [];
    private static char[] selectedAlphabet;

    private static readonly HashSet<string> UrlListOfStocksAndShares = [];
    private static readonly CompressedMemoryCache urlAndHtmlContentHashtable = new();
    private static readonly CompressedMemoryCache urlAndCompanyInfoHashtable = new();

    private static int _urlCounter;
    private static readonly object urlCounterLock = new();

    private static readonly HashSet<SEDOL> SEDOLs = [];

    internal static IEnumerable<ETFType>? ETF_Types;

    private CancellationTokenSource cancellationTokenSource;
    private static int _maxConnectionsPerServerSetting;
    private static readonly HttpClient HttpClient = new();

    public FrmMainApp()
    {
        cancellationTokenSource = new CancellationTokenSource();

        InitializeComponent();
        GetETFTypesFromCSV(); // read user-defined ETF types
        HelperDataDatabaseAndStartup.DataCreateSQLiteDB(); // make sure sqlite database exists (create if necessary)
        HelperDataDatabaseAndStartup.DataWriteSQLiteSettingsDefaultSettings(); // fill db w/ defaults where needed

        // get defaults 

        // re _maxConnectionsPerServerSetting: this is counted 2x in the code so a setting of 50 will send out 100 requests because
        // ... we're basically sending 1 for the "normal" page and 1 for the "coporate info" page.
        _maxConnectionsPerServerSetting = Convert.ToInt16(
            value: HelperDataApplicationSettings.DataReadSQLiteSettings(tableName: "settings",
                settingId: "MaxConnectionsPerServer"));
    }

    private void FrmMainApp_Load(object sender, EventArgs e)
    {
        if (HelperDataApplicationSettings.DataReadSQLiteSettings(tableName: "settings",
                settingId: "Theme") == "Dark")
        {
            tsmi_DarkishMode.PerformClick();
        }

        btn_StartScrape.Enabled = true;
        btn_Stop.Enabled = false;
        btn_ReloadCategories.Enabled = false;
        btn_SaveToCSV.Enabled = false;
        tpg_Overview.Enabled = false;

#if DEBUG
        lbx_Alphabet.SetSelected(index: lbx_Alphabet.FindStringExact(s: "y"), value: true);
#else
        for (int i = 0; i < lbx_Alphabet.Items.Count; i++)
        {
            lbx_Alphabet.SetSelected(index: i, value: true);
        }
#endif
    }

    #region Overview Tab

    /// <summary>
    ///     Fills (and refills/adjusts) the cbx dropdown's text options
    /// </summary>
    /// <param name="filter"></param>
    private void FillCbx_Securities(string filter = "")
    {
        cbx_Securities.SelectedValue = string.Empty;
        List<SEDOL> sedolList = SEDOLs.Where(predicate: sedol =>
                                           ((sedol.Is_ISA_Compatible && ckb_ISAOnlySearch.Checked) ||
                                            !ckb_ISAOnlySearch.Checked) &&
                                           ((sedol.ETF_Type != Not_ETF_ETFType && ckb_ETFOnlySearch.Checked) ||
                                            !ckb_ETFOnlySearch.Checked) &&
                                           (sedol.Name.ToLower().Contains(value: filter.ToLower()) ||
                                            string.IsNullOrWhiteSpace(value: filter)))
                                      .ToList();

        // Create a BindingSource
        BindingSource bindingSource = [];

        // Assign the data source to the binding source
        bindingSource.DataSource = sedolList;

        // Set the ComboBox's DataSource property to the BindingSource
        cbx_Securities.DataSource = bindingSource;

        // Set the DisplayMember and ValueMember properties of the ComboBox
        cbx_Securities.DisplayMember = "Name";
        cbx_Securities.ValueMember = "SEDOL_ID";
    }

    #endregion

    #region Scrape & Parse

    /// <summary>
    ///     Pulls the FX info and deserialises the JSON output
    /// </summary>
    /// <param name="formInstance"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private static async Task<bool> ReadJsonFXFromWebAsync(FrmMainApp formInstance,
        CancellationToken cancellationToken)
    {
        try
        {
            // Check if cancellation has been requested
            cancellationToken.ThrowIfCancellationRequested();

            string logMessageVal = "Reading FX Data From Web";
            AppendLogWindowText(tbx: formInstance.tbx_Log, appendText: logMessageVal,
                logMessageType: LogMessageTypes.Start);

            AppendLogWindowText(tbx: formInstance.tbx_Log,
                appendText: "Contacting FX Data website (http://www.floatrates.com/currency/gbp/)",
                logMessageType: LogMessageTypes.Info);


            HttpResponseMessage response =
                await HttpClient.GetAsync(requestUri: FxUrl, cancellationToken: cancellationToken);
            _ = response.EnsureSuccessStatusCode();

            string jsonString = await response.Content.ReadAsStringAsync(cancellationToken: cancellationToken);

            Dictionary<string, FXCurrency>? currencies =
                JsonConvert.DeserializeObject<Dictionary<string, FXCurrency>>(value: jsonString);

            Debug.Assert(condition: currencies != null, message: nameof(currencies) + " != null");
            foreach (KeyValuePair<string, FXCurrency> kvp in currencies)
            {
                FxCurrencies.Add(item: kvp.Value);
                AppendLogWindowText(tbx: formInstance.tbx_Log,
                    appendText: kvp.Value.Code + " " + kvp.Value.InverseRate);
            }

            AppendLogWindowText(tbx: formInstance.tbx_Log, appendText: logMessageVal,
                logMessageType: LogMessageTypes.Done);
            return true;
        }
        catch (Exception ex)
        {
            if (ex is OperationCanceledException or
                TaskCanceledException)
            {
                // Operation was cancelled
                AppendLogWindowText(tbx: formInstance.tbx_Log,
                    appendText: "Reading FX Data was cancelled. Process halted.",
                    logMessageType: LogMessageTypes.Info);
                return false;
            }

            AppendLogWindowText(tbx: formInstance.tbx_Log,
                appendText: "Failed to read or parse FX conversion values.",
                logMessageType: LogMessageTypes.Error);
            return false;
        }
    }

    /// <summary>
    ///     Loads up the URLs that belong to each letter of the alphabet
    /// </summary>
    /// <param name="formInstance"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private static async Task CollateHLStocksByLetterAsync(FrmMainApp formInstance,
        CancellationToken cancellationToken)
    {
        const string logMessageVal = "Building Initial list of stocks and shares";
        AppendLogWindowText(tbx: formInstance.tbx_Log,
            appendText:
            $"{logMessageVal} - please note that it can appear that the app goes quiet here/at the beginning of any/each block, it shouldn't actually be dead so be patient.",
            logMessageType: LogMessageTypes.Start);

        int totalUrls = UrlListOfStocksAndShares.Count;

        foreach (char alphabetChar in selectedAlphabet)
        {
            AppendLogWindowText(tbx: formInstance.tbx_Log, appendText: "Parsing block: " + alphabetChar,
                logMessageType: LogMessageTypes.None);
            bool timeOutBool = false;
            string respString = string.Empty;

            while (!timeOutBool)
            {
                try
                {
                    // Check if cancellation has been requested
                    cancellationToken.ThrowIfCancellationRequested();

                    string url = "https://www.hl.co.uk/shares/shares-search-results/" + alphabetChar;
                    HttpResponseMessage response =
                        await HttpClient.GetAsync(requestUri: url, cancellationToken: cancellationToken);
                    response.EnsureSuccessStatusCode();

                    respString = await response.Content.ReadAsStringAsync(cancellationToken: cancellationToken);
                    timeOutBool = true;
                }
                catch (Exception ex)
                {
                    if (ex is OperationCanceledException or
                        TaskCanceledException)
                    {
                        // Operation was cancelled
                        AppendLogWindowText(tbx: formInstance.tbx_Log,
                            appendText: "Parsing of alphabetical blocks was cancelled. Process halted.",
                            logMessageType: LogMessageTypes.Info);
                        return;
                    }


                    AppendLogWindowText(tbx: formInstance.tbx_Log, appendText: ex.Message,
                        logMessageType: LogMessageTypes.Error);
                    return;
                }
            }

            HtmlDocument doc = new();
            doc.LoadHtml(html: respString);

            HtmlNodeCollection? linkNodes = doc.DocumentNode.SelectNodes(xpath: "//a[@href]");

            if (linkNodes != null)
            {
                foreach (HtmlNode? linkNode in linkNodes)
                {
                    string href = linkNode.GetAttributeValue(name: "href", def: "");
#if DEBUG
                    //List<string> listOfURLsToDebug = new()
                    //{
                    //    "https://www.hl.co.uk/shares/shares-search-results/i/ishares-ii-plc-ftse-epranareit-asia-prop"
                    //};
                    List<string> listOfURLsToDebug = [];
                    if (href.Contains(value: "/shares/shares-search-results/" + alphabetChar + "/") &&
                        (listOfURLsToDebug.Any(predicate: href.Contains) ||
                         listOfURLsToDebug.Count == 0))
                    {
                        UrlListOfStocksAndShares.Add(item: href);
                    }
#else
                    if (href.Contains(value: "/shares/shares-search-results/" + alphabetChar + "/"))
                    {
                        UrlListOfStocksAndShares.Add(item: href);
                    }
#endif
                }
            }
        }

        AppendLogWindowText(tbx: formInstance.tbx_Log,
            appendText: "Collected " + UrlListOfStocksAndShares.Count + " items.",
            logMessageType: LogMessageTypes.None);

        await FireAndForgetAsync(urls: UrlListOfStocksAndShares, formInstance: formInstance,
            cancellationToken: cancellationToken);

        AppendLogWindowText(tbx: formInstance.tbx_Log, appendText: logMessageVal, logMessageType: LogMessageTypes.Done);
    }

    private static async Task FireAndForgetAsync(HashSet<string> urls, FrmMainApp formInstance,
        CancellationToken cancellationToken)
    {
        try
        {
            // Start scraping all items
            AppendLogWindowText(tbx: formInstance.tbx_Log, appendText: "Scraping all items.",
                logMessageType: LogMessageTypes.Start);

            // Chunk size for processing tasks
            int chunkSize = _maxConnectionsPerServerSetting;

            // Create a list to hold the tasks
            List<Task> tasks = [];

            // Iterate over the URLs in chunks
            for (int i = 0; i < urls.Count; i += chunkSize)
            {
                // Get the chunk of URLs
                IEnumerable<string> chunk = urls.Skip(count: i).Take(count: chunkSize);

                // Create a list to hold the tasks for this chunk
                List<Task> chunkTasks = [];

                // Create a new HttpClient for this chunk
                using HttpClient httpClient = new();

                // Loop through each URL in the chunk and start scraping
                foreach (string url in chunk)
                {
                    // Check if cancellation has been requested
                    cancellationToken.ThrowIfCancellationRequested();

                    // Construct the URL with "/company-information" appended
                    string companyInfoUrl = url + "/company-information";

                    // Add tasks to scrape the main URL and company info URL
                    chunkTasks.Add(item: GetHtmlAsyncWithClient(httpClient: httpClient, url: url,
                        formInstance: formInstance, cancellationToken: cancellationToken));
                    chunkTasks.Add(item: GetHtmlAsyncWithClient(httpClient: httpClient, url: companyInfoUrl,
                        formInstance: formInstance, cancellationToken: cancellationToken));
                }

                // Add chunk tasks to the main tasks list
                tasks.AddRange(collection: chunkTasks);

                // Wait for the chunk tasks to complete or cancellation requested
                await Task.WhenAll(tasks: chunkTasks);

                // Dispose of the HttpClient to close connections and release resources
                httpClient.Dispose();

                // Break loop if cancellation requested
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
            }

            // Scraping completed
            AppendLogWindowText(tbx: formInstance.tbx_Log, appendText: "Scraping all items.",
                logMessageType: LogMessageTypes.Done);
        }
        catch (Exception ex) when (ex is OperationCanceledException or TaskCanceledException)
        {
            // Tasks were cancelled
            AppendLogWindowText(tbx: formInstance.tbx_Log,
                appendText: "Scraping of web pages was cancelled. Process halted.",
                logMessageType: LogMessageTypes.Info);
        }
    }

    private static async Task GetHtmlAsyncWithClient(HttpClient httpClient, string url, FrmMainApp formInstance,
        CancellationToken cancellationToken)
    {
        // Check if cancellation has been requested
        cancellationToken.ThrowIfCancellationRequested();

        // Make the HTTP request
        HttpResponseMessage response = await httpClient.GetAsync(requestUri: url, cancellationToken: cancellationToken);
        string htmlContent = await response.Content.ReadAsStringAsync(cancellationToken: cancellationToken);

        // Process the HTML content
        // Page
        if (!url.Contains(value: "company-info"))
        {
            urlAndHtmlContentHashtable.AddOrUpdate(key: url,
                value: HelperStringUtils.TrimAndReplaceNewLinesAndTabs(
                    text: ReturnPageText(
                        HTMLTextInHtmlContentHashtable: HelperStringUtils.TrimInternalSpaces(s: htmlContent))));
        }
        // Company
        else
        {
            urlAndCompanyInfoHashtable.AddOrUpdate(key: url,
                value: HelperStringUtils.TrimAndReplaceNewLinesAndTabs(
                    text: ReturnCompanyPageText(
                        HTMLTextInCompanyInfoHashtable: HelperStringUtils.TrimInternalSpaces(s: htmlContent))));
        }

        IncrementCounterAndLogProgress(url: url, formInstance: formInstance, isSuccess: true);
    }

    /// <summary>
    ///     Creates the SEDOLs. Technically the primary key is the URL at the stage of creation.
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    private static SEDOL? CreateSEDOL(string url)
    {
        if (urlAndHtmlContentHashtable.ContainsKey(key: url) &&
            urlAndCompanyInfoHashtable.ContainsKey(key: url + "/company-information"))
        {
            Application.DoEvents();

            string HTMLTextInHtmlContentHashtable = urlAndHtmlContentHashtable.Get(key: url) ?? string.Empty;
            string HTMLTextInCompanyInfoHashtable =
                urlAndCompanyInfoHashtable.Get(key: url + "/company-information") ?? string.Empty;
            if (HTMLTextInHtmlContentHashtable.Length > 0 &&
                HTMLTextInCompanyInfoHashtable.Length > 0)
            {
                string logMessageVal = $"Parsing {url}";
                FrmMainApp frmMainAppInstance = (FrmMainApp)Application.OpenForms[name: "FrmMainApp"];
                //AppendLogWindowText(tbx: frmMainAppInstance.tbx_Log, appendText: logMessageVal,
                //    logMessageType: LogMessageTypes.Start);

                string pageText = HTMLTextInHtmlContentHashtable;
                string companyPageText = HTMLTextInCompanyInfoHashtable;

                if (pageText.Contains(
                        value:
                        "By law certain stocks must have a Key Investor Information Document / Key Information Document available before investors can purchase them."))
                {
                    AppendLogWindowText(tbx: frmMainAppInstance.tbx_Log, appendText: logMessageVal + " - invalid item.",
                        logMessageType: LogMessageTypes.Error);
                    return null;
                }

                string name = TagsToModelValueTransformations.T2M_Name(pageText: pageText);
                string sedolID = TagsToModelValueTransformations.T2M_SEDOL_ID(pageText: pageText);
                string ticker = TagsToModelValueTransformations.T2M_Ticker(name: name);

                // exit if fails here
                if (string.IsNullOrWhiteSpace(value: sedolID) ||
                    string.IsNullOrWhiteSpace(value: ticker) ||
                    string.IsNullOrWhiteSpace(value: name) ||
                    name.Contains(value: "404"))
                {
                    AppendLogWindowText(tbx: frmMainAppInstance.tbx_Log, appendText: logMessageVal + " - invalid item.",
                        logMessageType: LogMessageTypes.Error);
                    return null;
                }

                string currISO3 = TagsToModelValueTransformations.T2M_Currency(pageText: pageText);
                string currSign = TagsToModelValueTransformations.T2M_CurrencySign(currISO3: currISO3);
                double gbpEqivalent = TagsToModelValueTransformations.T2M_GBPEquivalent(currISO3: currISO3);
                double openVal = TagsToModelValueTransformations.T2M_Open_price(pageText: pageText);
                double yearLow = TagsToModelValueTransformations.T2M_Year_low(pageText: pageText, openVal: openVal);
                double yearHigh = TagsToModelValueTransformations.T2M_Year_high(pageText: pageText, openVal: openVal);
                double volume = TagsToModelValueTransformations.T2M_Volume(pageText: pageText);
                double marketCap =
                    TagsToModelValueTransformations.T2M_Market_capitalisation(pageText: pageText, currISO3: currISO3,
                        currSign: currSign);

                // this is a bit tricky. bonds & trusts don't _really_ have a corporate page so i have to trick around this stuff
                string sector = pageText.Contains(value: "More about bond pricing here")
                    ? "Bond"
                    : pageText.Contains(value: "Trust&nbsp;<br/>info")
                    ? "Trust"
                    : TagsToModelValueTransformations.T2M_Sector(companyPageText: companyPageText,
                        securityNameLowerCase: name, ticker: ticker, marketCapOverZero: marketCap > 0);
                string etfType = !sector.Contains(value: "ETF")
                    ? Not_ETF_ETFType
                    : TagsToModelValueTransformations.T2M_ETF_Type(name: name);
                SEDOL newSedol = new()
                {
                    URL = url,
                    Name = name,
                    Is_ISA_Compatible = pageText.Contains(value: "icon-link tick-icon small-margin-right"),
                    SEDOL_ID = sedolID,
                    Currency = currISO3,
                    Ticker = ticker,
                    Open_price = openVal,
                    Year_low = yearLow,
                    Year_high = yearHigh,
                    Volume = volume,
                    Dividend_yield = TagsToModelValueTransformations.T2M_Dividend_yield(pageText: pageText),
                    PE_ratio = TagsToModelValueTransformations.T2M_PE_ratio(pageText: pageText, currSign: currSign),
                    Market_capitalisation = marketCap,
                    GBP_Open = openVal * gbpEqivalent,
                    GBP_Year_low = yearLow * gbpEqivalent,
                    GBP_Year_high = yearHigh * gbpEqivalent,
                    GBP_Market_capitalisation = marketCap * gbpEqivalent,
                    Sector = sector,
                    ETF_Type = etfType,
                    Top10_Exposures = TagsToModelValueTransformations.T2M_Top10_Exposures(pageText: pageText),
                    Exchange = TagsToModelValueTransformations.T2M_Exchange(companyPageText: companyPageText),
                    Country = TagsToModelValueTransformations.T2M_Country(companyPageText: companyPageText),
                    Indices = TagsToModelValueTransformations.T2M_Indices(companyPageText: companyPageText)
                };

                AppendLogWindowText(tbx: frmMainAppInstance.tbx_Log,
                    appendText: logMessageVal + $" - {name} - {sedolID}.",
                    logMessageType: LogMessageTypes.Done);

                return newSedol;
            }
        }
        else
        {
            return null;
        }

        return null;
    }

    /// <summary>
    ///     Gets the relevant parts of the main page text. Each html file is some 200-400K and most of it is just
    ///     useless/script/whitespace/other junk.
    /// </summary>
    /// <param name="HTMLTextInHtmlContentHashtable"></param>
    /// <returns></returns>
    private static string ReturnPageText(string HTMLTextInHtmlContentHashtable)
    {
        string[] textsToLookFor =
        [
            "<div class",
            "<span data-tooltip",
            "<br class=",
            "<span class",
            "</span"
        ];

        string pageText = "<h1>" + HelperStringUtils.FindTextBetween(pageText: HTMLTextInHtmlContentHashtable,
            textStart: "<h1>",
            textEnd: "<!-- end factsheet -->");

        for (int i = 0; i < 5; i++)
        {
            string textToLookFor = textsToLookFor[i];
            int count = (pageText.Length - pageText.Replace(oldValue: textToLookFor, newValue: "").Length) /
                        textToLookFor.Length;

            for (int i2 = 0; i2 < count; i2++)
            {
                int charToFindStart =
                    pageText.IndexOf(value: textToLookFor, comparisonType: StringComparison.Ordinal);
                int charToFindEnd = pageText.IndexOf(value: ">", startIndex: charToFindStart,
                    comparisonType: StringComparison.Ordinal);
                if (charToFindStart != -1 &&
                    charToFindEnd != -1)
                {
                    pageText = pageText.Remove(startIndex: charToFindStart,
                        count: charToFindEnd - charToFindStart + 1);
                }
            }
        }

        pageText = pageText.Replace(oldValue: "Market capitalisation</div>Market cap.</div>",
            newValue: "Market capitalisation:");

        return pageText;
    }

    /// <summary>
    ///     Gets the relevant part of the corporate info subpages.
    ///     This particular one has been ported from VBA more or less as-was so it's a little redundant in its ways but
    ///     nonetheless efficient at cleaning up stuff.
    /// </summary>
    /// <param name="HTMLTextInCompanyInfoHashtable"></param>
    /// <returns></returns>
    private static string ReturnCompanyPageText(string HTMLTextInCompanyInfoHashtable)
    {
        string trimmedText = HTMLTextInCompanyInfoHashtable.Trim();
        bool crapIndicator = false;
        string pageText = "<!DOCTYPE html><head></head><body>";
        int charToFindEnd = -1;
        int charToFindStart =
            trimmedText.IndexOf(value: "<strong>EPIC:</strong>", comparisonType: StringComparison.Ordinal);
        if (charToFindStart == -1)
        {
            charToFindStart = trimmedText.IndexOf(value: "<strong>Short code:</strong>",
                comparisonType: StringComparison.Ordinal);
            if (charToFindStart == -1)
            {
                crapIndicator = true;
            }
        }

        if (!crapIndicator)
        {
            charToFindEnd = trimmedText.IndexOf(value: "</dd>", startIndex: charToFindStart,
                comparisonType: StringComparison.Ordinal);
            pageText += trimmedText.Substring(startIndex: charToFindStart,
                length: charToFindEnd - charToFindStart + "</dd>".Length);
        }

        if (!crapIndicator)
        {
            charToFindStart =
                trimmedText.IndexOf(value: "<strong>Sector:</strong>", startIndex: charToFindEnd,
                    comparisonType: StringComparison.Ordinal);
            if (charToFindStart == -1)
            {
                crapIndicator = true;
            }
        }

        if (!crapIndicator)
        {
            charToFindEnd = trimmedText.IndexOf(value: "</dd>", startIndex: charToFindStart,
                comparisonType: StringComparison.Ordinal);
            pageText += trimmedText.Substring(startIndex: charToFindStart,
                length: charToFindEnd - charToFindStart + "</dd>".Length);
        }

        if (!crapIndicator)
        {
            charToFindStart =
                trimmedText.IndexOf(value: "<strong>Exchange:</strong>", startIndex: charToFindEnd,
                    comparisonType: StringComparison.Ordinal);
            if (charToFindStart == -1)
            {
                crapIndicator = true;
            }
        }

        if (!crapIndicator)
        {
            charToFindEnd = trimmedText.IndexOf(value: "</dd>", startIndex: charToFindStart,
                comparisonType: StringComparison.Ordinal);
            pageText += trimmedText.Substring(startIndex: charToFindStart,
                length: charToFindEnd - charToFindStart + "</dd>".Length);
        }

        if (!crapIndicator)
        {
            charToFindStart =
                trimmedText.IndexOf(value: "<strong>Country:</strong>", startIndex: charToFindEnd,
                    comparisonType: StringComparison.Ordinal);
            if (charToFindStart == -1)
            {
                crapIndicator = true;
            }
        }

        if (!crapIndicator)
        {
            charToFindEnd = trimmedText.IndexOf(value: "</dd>", startIndex: charToFindStart,
                comparisonType: StringComparison.Ordinal);
            pageText += trimmedText.Substring(startIndex: charToFindStart,
                length: charToFindEnd - charToFindStart + "</dd>".Length);
        }

        if (!crapIndicator)
        {
            charToFindStart =
                trimmedText.IndexOf(value: "<strong>Indices:</strong>", startIndex: charToFindEnd,
                    comparisonType: StringComparison.Ordinal);
            if (charToFindStart == -1)
            {
                crapIndicator = true;
            }
        }

        if (!crapIndicator)
        {
            charToFindEnd = trimmedText.IndexOf(value: "</dd>", startIndex: charToFindStart,
                comparisonType: StringComparison.Ordinal);
            pageText += trimmedText.Substring(startIndex: charToFindStart,
                length: charToFindEnd - charToFindStart + "</dd>".Length);
        }

        if (crapIndicator)
        {
            pageText += "Crap Data";
        }

        pageText = pageText.Replace(oldValue: "<strong>", newValue: "")
                           .Replace(oldValue: "</strong>", newValue: "");
        pageText += "</body>";

        return pageText;
    }

    #endregion

    #region Events

    #region Generic

    /// <summary>
    ///     Fires the process to save the findings to a CSV
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btn_SaveToCSV_Click(object sender, EventArgs e)
    {
        WriteOutputToCSV();
    }

    /// <summary>
    ///     Reads the CSV file for ETF categories
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btn_ReloadCategories_Click(object sender, EventArgs e)
    {
        GetETFTypesFromCSV();
        _ = MessageBox.Show(text: "Categories reloaded.");
        foreach (SEDOL sedol in SEDOLs)
        {
            string name = sedol.Name;
            sedol.ETF_Type = !sedol.Sector.Contains(value: "ETF")
                ? Not_ETF_ETFType
                : TagsToModelValueTransformations.T2M_ETF_Type(name: name);
        }
    }

    /// <summary>
    ///     Loads the About Form
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void tsm_About_Click(object sender, EventArgs e)
    {
        FrmAboutBox frmAboutBox = new();
        _ = frmAboutBox.ShowDialog();
    }

    /// <summary>
    ///     Open the ETF_Types.csv in the external editor
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void tsmi_EditCategorisationCSV_Click(object sender, EventArgs e)
    {
        // copy the Example file if "normal" doesn't exist.
        if (!File.Exists(path: Path.Combine(path1: HelperVariables.GetResourcesFolderString(), path2: "ETF_Types.csv")))
        {
            File.Copy(
                sourceFileName: Path.Combine(path1: HelperVariables.GetResourcesFolderString(),
                    path2: "ETF_Types_Example.csv"),
                destFileName: Path.Combine(path1: HelperVariables.GetResourcesFolderString(), path2: "ETF_Types.csv"));
        }

        _ = new Process
        {
            StartInfo = new ProcessStartInfo(fileName: Path.Combine(path1: HelperVariables.GetResourcesFolderString(),
                path2: "ETF_Types.csv"))
            {
                UseShellExecute = true
            }
        }.Start();
    }


    private void tsmi_DarkishMode_Click(object sender, EventArgs e)
    {
        HelperVariables.UserSettingUseDarkMode = tsmi_DarkishMode.Checked;
        SetAppTheme();

        HelperDataApplicationSettings.DataWriteSQLiteSettings(tableName: "settings", settingId: "Theme",
            settingValue: HelperVariables.UserSettingUseDarkMode ? "Dark" : "Light");
    }

    private void FrmMainApp_FormClosing(object sender, FormClosingEventArgs e)
    {
        HelperDataApplicationSettings.DataDeleteSQLitesettingsCleanup();
        HelperDataApplicationSettings.DataVacuumDatabase();
    }

    #endregion

    #region tpg_ScrapeMainGrid

    /// <summary>
    ///     Commences the scrape
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void btn_StartScrape_Click(object sender, EventArgs e)
    {
        selectedAlphabet = lbx_Alphabet.SelectedItems.Cast<char>().ToArray();
        if (selectedAlphabet.Length > 0)
        {
            // reset things.
            btn_StartScrape.Enabled = false;
            btn_Stop.Enabled = true;
            btn_ReloadCategories.Enabled = false;
            btn_SaveToCSV.Enabled = false;
            tpg_Overview.Enabled = false;

            tbx_Log.Clear();
            FxCurrencies.Clear();
            urlAndHtmlContentHashtable.Clear();
            urlAndCompanyInfoHashtable.Clear();
            UrlListOfStocksAndShares.Clear();

            _urlCounter = 0;

            cancellationTokenSource = new CancellationTokenSource();

            if (await ReadJsonFXFromWebAsync(formInstance: this, cancellationToken: cancellationTokenSource.Token))
            {
                await CollateHLStocksByLetterAsync(formInstance: this,
                    cancellationToken: cancellationTokenSource.Token);
            }

            foreach (SEDOL? newSedol in UrlListOfStocksAndShares.Select(selector: CreateSEDOL).OfType<SEDOL>())
            {
                if (!string.IsNullOrWhiteSpace(value: newSedol?.SEDOL_ID))
                {
                    _ = SEDOLs.Add(item: newSedol);
                }
            }

            btn_StartScrape.Enabled = true;
            btn_Stop.Enabled = false;
            btn_ReloadCategories.Enabled = true;
            if (SEDOLs.Count > 0)
            {
                btn_SaveToCSV.Enabled = true;
                tpg_Overview.Enabled = true;
                FrmMainApp frmMainAppInstance = (FrmMainApp)Application.OpenForms[name: "FrmMainApp"];
                string logMessageVal = "Building parsing data - ready for output.";
                AppendLogWindowText(tbx: frmMainAppInstance.tbx_Log, appendText: logMessageVal,
                    logMessageType: LogMessageTypes.Done);
                nic_ProcessFinished.ShowBalloonTip(timeout: 150);
            }
            else
            {
                FrmMainApp frmMainAppInstance = (FrmMainApp)Application.OpenForms[name: "FrmMainApp"];
                string logMessageVal = "Building parsing data encountered an error. No data was parsed.";
                AppendLogWindowText(tbx: frmMainAppInstance.tbx_Log, appendText: logMessageVal,
                    logMessageType: LogMessageTypes.Error);
            }

            // release memory
            urlAndHtmlContentHashtable.Clear();
            urlAndCompanyInfoHashtable.Clear();
        }
        else
        {
            _ = MessageBox.Show(text: "You do need to select at least one character you know.",
                caption: "Nothing selected.",
                buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Warning);
        }
    }


    /// <summary>
    ///     Sends a CT to the ongoing process
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btn_Stop_Click(object sender, EventArgs e)
    {
        // Check if cancellationTokenSource is initialized and if there's an ongoing task
        if (cancellationTokenSource != null &&
            !cancellationTokenSource.IsCancellationRequested)
        {
            // Cancel the ongoing tasks
            cancellationTokenSource.Cancel();
        }

        btn_StartScrape.Enabled = true;
        btn_Stop.Enabled = false;
        btn_ReloadCategories.Enabled = false;
        btn_SaveToCSV.Enabled = false;
    }

    #endregion

    #region tpg_Overview

    private void tpg_Overview_Enter(object sender, EventArgs e)
    {
        FillCbx_Securities();
    }

    private void tbx_Search_TextChanged(object sender, EventArgs e)
    {
        FillCbx_Securities(filter: tbx_Search.Text);
    }

    private void ckb_ISAOnlySearch_CheckedChanged(object sender, EventArgs e)
    {
        FillCbx_Securities(filter: tbx_Search.Text);
    }

    private void ckb_ETFOnlySearch_CheckedChanged(object sender, EventArgs e)
    {
        FillCbx_Securities(filter: tbx_Search.Text);
    }

    private void cbx_Securities_SelectedValueChanged(object sender, EventArgs e)
    {
        if (cbx_Securities.SelectedItem is SEDOL selectedSEDOL)
        {
            // Access the value member (SEDOLID) of the selected item
            string selectedSedolSedolId = selectedSEDOL.SEDOL_ID;

            // Find the SEDOL object in sedols where SEDOLID matches x
            SEDOL sedol = SEDOLs.FirstOrDefault(predicate: s => s.SEDOL_ID == selectedSedolSedolId);

            if (sedol != null)
            {
                tbx_Name.Text = sedol.Name;
                tbx_Ticker.Text = sedol.Ticker;
                tbx_SEDOL.Text = sedol.SEDOL_ID;
                ckb_ISA.Checked = sedol.Is_ISA_Compatible;
                ckb_ISA.Text = $"{(sedol.Is_ISA_Compatible ? "Can" : "Can't")} be held in an ISA";
                llb_URL.Links.Clear();
                _ = llb_URL.Links.Add(start: 0, length: 3, linkData: sedol.URL);
                tbx_Sector.Text = sedol.Sector;
                tbx_ETFType.Text = sedol.ETF_Type;
                tbx_Country.Text = sedol.Country;
                tbx_Currency.Text = sedol.Currency;
                tbx_Indicies.Text = sedol.Indices;
                tbx_DivYield.Text = sedol.Dividend_yield;
                tbx_OpenPrice_OC.Text = sedol.Open_price.ToString(format: "N4", provider: NumberFormatInfo.CurrentInfo);
                tbx_OpenPrice_GBP.Text =
                    sedol.GBP_Open.ToString(format: "C4", provider: new CultureInfo(name: "en-GB"));
                tbx_YearLow_OC.Text = sedol.Year_low.ToString(format: "N4", provider: NumberFormatInfo.CurrentInfo);
                tbx_YearLow_GBP.Text =
                    sedol.GBP_Year_low.ToString(format: "C4", provider: new CultureInfo(name: "en-GB"));
                tbx_YearHigh_OC.Text = sedol.Year_high.ToString(format: "N4", provider: NumberFormatInfo.CurrentInfo);
                tbx_YearHigh_GBP.Text =
                    sedol.GBP_Year_high.ToString(format: "C4", provider: new CultureInfo(name: "en-GB"));
                tbx_MarketCap_OC.Text =
                    sedol.Market_capitalisation.ToString(format: "N0", provider: NumberFormatInfo.CurrentInfo);
                tbx_MarketCap_GBP.Text =
                    sedol.GBP_Market_capitalisation.ToString(format: "C0", provider: new CultureInfo(name: "en-GB"));
                tbx_PE.Text = sedol.PE_ratio.ToString(format: "N4", provider: NumberFormatInfo.CurrentInfo);
                tbx_Volume.Text = sedol.Volume.ToString(format: "N0", provider: NumberFormatInfo.CurrentInfo);
            }
        }
    }

    private void llb_URL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        if (e.Link != null)
        {
            _ = Process.Start(startInfo: new ProcessStartInfo(fileName: e.Link.LinkData.ToString())
            { UseShellExecute = true });
        }
    }

    #endregion

    #endregion

    #region Utils

    /// <summary>
    ///     Puts text into the log window (append)
    /// </summary>
    /// <param name="tbx"></param>
    /// <param name="appendText"></param>
    /// <param name="logMessageType"></param>
    private static void AppendLogWindowText(TextBox tbx, string appendText,
        LogMessageTypes logMessageType = LogMessageTypes.None)
    {
        string messageType = logMessageType != LogMessageTypes.None ? "[" + logMessageType + "] " : "";

        if (tbx.InvokeRequired)
        {
            _ = tbx.Invoke(method: (MethodInvoker)delegate
            {
                tbx.AppendText(text: DateTime.Now + " - " + messageType + appendText +
                                     Environment.NewLine);
                tbx.SelectionStart = tbx.Text.Length;
                tbx.ScrollToCaret();
            });
        }
        else
        {
            tbx.AppendText(text: DateTime.Now + " - " + messageType + appendText +
                                 Environment.NewLine);
            tbx.SelectionStart = tbx.Text.Length;
            tbx.ScrollToCaret();
        }
    }


    /// <summary>
    ///     Logs the count of URLs parsed and returns the result message accordingly
    /// </summary>
    /// <param name="url"></param>
    /// <param name="formInstance"></param>
    /// <param name="isSuccess"></param>
    /// <param name="errorMsg"></param>
    private static void IncrementCounterAndLogProgress(string url, FrmMainApp formInstance, bool isSuccess,
        string errorMsg = "")
    {
        lock (urlCounterLock)
        {
            _urlCounter++;
            if (isSuccess)
            {
                AppendLogWindowText(tbx: formInstance.tbx_Log,
                    appendText: $"{_urlCounter} of {UrlListOfStocksAndShares.Count * 2} items scraped - {url}.",
                    logMessageType: LogMessageTypes.None);
            }
            else
            {
                AppendLogWindowText(tbx: formInstance.tbx_Log,
                    appendText: $"Error fetching URL '{url}': {errorMsg}",
                    logMessageType: LogMessageTypes.Error);
            }
        }
    }


    /// <summary>
    ///     Sets the application theme at startup based on the user's settings.
    /// </summary>
    /// <remarks>
    ///     If the user has chosen to use dark mode, the method sets the theme color to dark and applies a custom renderer to
    ///     the menu strip.
    ///     If the user has not chosen to use dark mode, the method sets the theme color to light and uses the default
    ///     rendering for the controls.
    /// </remarks>
    private void SetAppTheme()
    {
        // the custom logic is ugly af so no need to be pushy about it in light mode.
        if (!HelperVariables.UserSettingUseDarkMode)
        {
            tcr_Main.DrawMode = TabDrawMode.Normal;
            mns_Main.RenderMode = ToolStripRenderMode.System;
        }
        else
        {
            mns_Main.Renderer = new DarkMenuStripRenderer();
        }

        // adds colour/theme

        HelperControlThemeManager.SetThemeColour(
            themeColour: HelperVariables.UserSettingUseDarkMode
                ? ThemeColour.Dark
                : ThemeColour.Light, parentControl: this);
    }


    // via https://stackoverflow.com/questions/9260303/how-to-change-menu-hover-color
    private class DarkMenuStripRenderer : ToolStripProfessionalRenderer
    {
        public DarkMenuStripRenderer() : base(professionalColorTable: new DarkColours())
        {
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            e.TextColor = Color.White;

            base.OnRenderItemText(e: e);
        }
    }


    private class DarkColours : ProfessionalColorTable
    {
        public override Color MenuItemBorder =>
            ColorTranslator.FromHtml(htmlColor: "#BAB9B9");

        public override Color MenuBorder =>
            Color.Silver; //added for changing the menu border

        public override Color MenuItemPressedGradientBegin =>
            ColorTranslator.FromHtml(htmlColor: "#4C4A48");

        public override Color MenuItemPressedGradientEnd =>
            ColorTranslator.FromHtml(htmlColor: "#5F5D5B");

        public override Color ToolStripBorder =>
            ColorTranslator.FromHtml(htmlColor: "#4C4A48");

        public override Color MenuItemSelectedGradientBegin =>
            ColorTranslator.FromHtml(htmlColor: "#4C4A48");

        public override Color MenuItemSelectedGradientEnd =>
            ColorTranslator.FromHtml(htmlColor: "#5F5D5B");

        public override Color MenuItemSelected =>
            ColorTranslator.FromHtml(htmlColor: "#5F5D5B");

        public override Color ToolStripDropDownBackground =>
            ColorTranslator.FromHtml(htmlColor: "#404040");

        public override Color ToolStripGradientBegin =>
            ColorTranslator.FromHtml(htmlColor: "#404040");

        public override Color ToolStripGradientEnd =>
            ColorTranslator.FromHtml(htmlColor: "#404040");

        public override Color ToolStripGradientMiddle =>
            ColorTranslator.FromHtml(htmlColor: "#404040");

        public override Color ImageMarginGradientBegin =>
            ColorTranslator.FromHtml(htmlColor: "#404040");

        public override Color ImageMarginGradientEnd =>
            ColorTranslator.FromHtml(htmlColor: "#404040");

        public override Color ImageMarginGradientMiddle =>
            ColorTranslator.FromHtml(htmlColor: "#404040");

        public override Color ImageMarginRevealedGradientBegin =>
            ColorTranslator.FromHtml(htmlColor: "#404040");

        public override Color ImageMarginRevealedGradientEnd =>
            ColorTranslator.FromHtml(htmlColor: "#404040");

        public override Color ImageMarginRevealedGradientMiddle =>
            ColorTranslator.FromHtml(htmlColor: "#404040");
    }

    #endregion

    #region CSV

    /// <summary>
    ///     Loads the ETF_Types from the CSV file
    /// </summary>
    private static void GetETFTypesFromCSV()
    {
        string resourcesFolder = Path.Combine(path1: AppDomain.CurrentDomain.BaseDirectory,
            path2: "Resources");
        string ETF_TypesCSVPath = File.Exists(path: Path.Combine(path1: resourcesFolder, path2: "ETF_Types.csv"))
            ? Path.Combine(path1: resourcesFolder, path2: "ETF_Types.csv")
            : Path.Combine(path1: resourcesFolder, path2: "ETF_Types_Example.csv");

        using FileStream stream = new(ETF_TypesCSVPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using StreamReader reader = new(stream);
        using CsvReader csv = new(reader, CultureInfo.InvariantCulture);
        ETF_Types = csv.GetRecords<ETFType>().ToList();
    }

    /// <summary>
    ///     Writes the scraping output to a CSV file in the Output folder
    /// </summary>
    private void WriteOutputToCSV()
    {
        string outputFolder = Path.Combine(path1: AppDomain.CurrentDomain.BaseDirectory,
            path2: "Output");
        _ = Directory.CreateDirectory(path: outputFolder);

        string nowDTStr = DateTime.Now.ToString(format: "yyyyMMdd_HHmmss");

        // if filters are active ask user if they want to use selection
        DialogResult dialogResult = DialogResult.No;
        if (cbx_Securities.Items.Count > 0 &&
            cbx_Securities.Items.Count < SEDOLs.Count &&
            tcr_Main.SelectedTab == tpg_Overview)
        {
            dialogResult = MessageBox.Show(text: "Only export filtered items?",
                caption: "Filters are active.",
                buttons: MessageBoxButtons.YesNoCancel, icon: MessageBoxIcon.Question);
        }

        List<SEDOL> sedolList = [];
        if (dialogResult == DialogResult.Yes)
        {
            foreach (object item in cbx_Securities.Items)
            {
                if (item is SEDOL selectedSEDOL)
                {
                    sedolList.Add(item: selectedSEDOL);
                }
            }
        }
        else if (dialogResult == DialogResult.No)
        {
            foreach (SEDOL sedol in SEDOLs)
            {
                sedolList.Add(item: sedol);
            }
        }

        if (dialogResult != DialogResult.Cancel)
        {
            try
            {
                using StreamWriter writer =
                    new(path: Path.Combine(path1: outputFolder, path2: $"HLWebScraper_Output_{nowDTStr}.csv"));
                using CsvWriter csv = new(writer: writer, culture: CultureInfo.InvariantCulture);
                csv.WriteHeader<SEDOL>();
                csv.NextRecord();
                foreach (SEDOL record in SEDOLs.Where(predicate: record =>
                             (dialogResult == DialogResult.Yes && sedolList.Contains(item: record)) ||
                             dialogResult == DialogResult.No))
                {
                    csv.WriteRecord(record: record);
                    csv.NextRecord();
                }

                writer.Flush();
                _ = MessageBox.Show(text: $"Finished writing the CSV file - HLWebScraper_Output_{nowDTStr}.csv",
                    caption: "Ok writing CSV",
                    buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show(text: $"Failed to write the CSV file - {ex.Message}", caption: "Error writing CSV",
                    buttons: MessageBoxButtons.OK,
                    icon: MessageBoxIcon.Error);
            }
        }
    }

    #endregion
}

internal enum LogMessageTypes
{
    Start,
    Done,
    Error,
    Info,
    None
}