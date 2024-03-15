using System.Text.RegularExpressions;
using HLWebScraper.Net.Helpers;

namespace HLWebScraper.Net.Model;

internal static class TagsToModelValueTransformations
{
    /// <summary>
    ///     Gets the name of the security from the header. Not using <title></title> because that has some extra crap in it.
    ///     Vaguely similar logic with the <span></span> vs <h1></h1> f..kery. In some cases relying on span returns odd
    ///     values.
    /// </summary>
    /// <param name="pageText">The HTML string</param>
    /// <returns></returns>
    public static string T2M_Name(string pageText)
    {
        string nameViaH1Span =
            HelperStringUtils.FindTextBetween(pageText: pageText, textStart: "<h1>", textEnd: "<span>");
        string nameViaH1H1 = HelperStringUtils.FindTextBetween(pageText: pageText, textStart: "<h1>", textEnd: "</h1>");
        return HelperStringUtils.ClearUTFChars(input: nameViaH1Span.Length < nameViaH1H1.Length
            ? nameViaH1Span
            : nameViaH1H1);
    }

    //public static bool T2M_Is_ISA_Compatible(string pageText)
    //{
    //}

    /// <summary>
    ///     Gets the ticker if one's available
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string T2M_Ticker(string name)
    {
        // see if there's a "(xxx)" style text in there somewhere.
        try
        {
            return name.Substring(startIndex: name.LastIndexOf(value: "(", comparisonType: StringComparison.Ordinal),
                            length: name.LastIndexOf(value: ")", comparisonType: StringComparison.Ordinal) -
                            name.LastIndexOf(value: "(", comparisonType: StringComparison.Ordinal) + 1)
                       .Replace(oldValue: "(", newValue: "").Replace(oldValue: ")", newValue: "");
        }
        catch
        {
            return "";
        }
    }

    /// <summary>
    ///     Gets the SEDOL ID. If this fails the newSedol won't be added to the SEDOLs Hashset
    /// </summary>
    /// <param name="pageText"></param>
    /// <returns></returns>
    public static string T2M_SEDOL_ID(string pageText)
    {
        string sedolViaID_SEDOL =
            HelperStringUtils.FindTextBetween(pageText: pageText, textStart: "ID_SEDOL=", textEnd: "&");
        string sedolViaID_NOTATION =
            HelperStringUtils.FindTextBetween(pageText: pageText, textStart: "ID_NOTATION=", textEnd: "&");

        return HelperStringUtils.ClearUTFChars(input: sedolViaID_SEDOL != string.Empty
            ? sedolViaID_SEDOL
            : sedolViaID_NOTATION);
    }

    /// <summary>
    ///     Somewhat arbitrarily attempts to figure if something is stuff like Gilts or an ETF or not. If likely neither then
    ///     we try to pull the Sector info from teh company-info page.
    ///     Items that have a non-zero market cap are definitely not ETFs
    /// </summary>
    /// <param name="companyPageText"></param>
    /// <param name="securityNameLowerCase"></param>
    /// <param name="ticker"></param>
    /// <param name="marketCapOverZero"></param>
    /// <returns></returns>
    public static string T2M_Sector(string companyPageText, string securityNameLowerCase, string ticker,
        bool marketCapOverZero)
    {
        securityNameLowerCase = securityNameLowerCase.ToLower();
        string sector = "Unspecified";

        // Items that have a non - zero market cap are definitely not ETFs
        if (!marketCapOverZero)
        {
            List<string> invalidatingContainerList = new() { " ord ", " ordinary ", " npv", "stk", ".0", ".1" };
            if (!invalidatingContainerList.Any(predicate: container =>
                    securityNameLowerCase.Contains(value: container)))
            {
                List<string> etf1xContainerList = new()
                {
                    "daily ", " daily", "invesco", "etf ", " etf", "fund ", " fund", "ucits", "msci", "accum", "etc",
                    "ishares", "growth", "lyxor", "commodity", "index", "wisdomtree", "wisdom tree", "gold bu",
                    "xtrackers", "multi unit", "xbt provider"
                };
                if (securityNameLowerCase.Contains(value: "gilt"))
                    sector = "Gilts etc";

                if (etf1xContainerList.Any(predicate: container =>
                        securityNameLowerCase.Contains(value: container)))
                    sector = "ETF 1x";


                if (securityNameLowerCase.Contains(value: "jp morgan") &&
                    ticker != "JPM")
                    sector = "ETF 1x";

                if (securityNameLowerCase.Contains(value: "-1x"))
                    sector = "ETF -1x";

                if (securityNameLowerCase.Contains(value: "5x"))
                    sector = "ETF 5x";

                if (securityNameLowerCase.StartsWith(value: "double short"))
                    sector = "ETF -2x";

                if (securityNameLowerCase.Contains(value: "2x"))
                    sector = "ETF 2x";

                if (securityNameLowerCase.Contains(value: "-2x"))
                    sector = "ETF -2x";

                if (securityNameLowerCase.Contains(value: "2.25x"))
                    sector = "ETF 2x";

                if (securityNameLowerCase.Contains(value: "3x"))
                    sector = "ETF 3x";

                if (securityNameLowerCase.Contains(value: "4x"))
                    sector = "ETF 4x";

                if (securityNameLowerCase.Contains(value: "%"))
                    sector = "Gilts etc";

                // if starts with ETF, ends with x, contains (short OR inverse), doesn't contain a negative ("-") or a bunch of other words then 
                if (securityNameLowerCase.Contains(value: "short") ||
                    (securityNameLowerCase.Contains(value: "inverse") &&
                     sector.StartsWith(value: "ETF") && sector.EndsWith(value: "x") &&
                     !(sector.StartsWith(value: "ETF -") && sector.EndsWith(value: "x"))))
                {
                    List<string> ignoreContainsList = new()
                    {
                        "short term",
                        "short-term",
                        "matur",
                        "ultra",
                        "duration"
                    };
                    if (!ignoreContainsList.Any(predicate: container => sector.Contains(value: container)))
                        sector = sector.Replace(oldValue: "ETF ", newValue: "ETF -");
                }
            }
        }

        // try and pull from website. this might be a logic first step but i'd like to qualify etfs separately.
        string likelySector = string.Empty;
        if (sector == "Unspecified")
            likelySector = HelperStringUtils.FindTextBetween(
                pageText: companyPageText,
                textStart: "Sector:<dd>",
                textEnd: "</dd>");

        return HelperStringUtils.ClearUTFChars(input: string.IsNullOrWhiteSpace(value: likelySector)
            ? sector.Replace(oldValue: "&", newValue: "and")
            : likelySector);
    }

    /// <summary>
    ///     This reads from the local CSV and marries up ETF categories with keywords
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string T2M_ETF_Type(string name)
    {
        foreach (ETFType etfType in FrmMainApp.ETF_Types)
            if (name.ToLower().Contains(value: etfType.Keyword.ToLower()))
                return etfType.ETF_Type;


        return "Not classified";
    }

    public static string T2M_Exchange(string companyPageText)
    {
        return HelperStringUtils.ClearUTFChars(input: HelperStringUtils.FindTextBetween(
            pageText: companyPageText,
            textStart: "Exchange:<dd>",
            textEnd: "</dd>"));
    }

    public static string T2M_Country(string companyPageText)
    {
        return HelperStringUtils.ClearUTFChars(input: HelperStringUtils.FindTextBetween(
            pageText: companyPageText,
            textStart: "Country:<dd>",
            textEnd: "</dd>"));
    }

    public static string T2M_Indices(string companyPageText)
    {
        return HelperStringUtils.ClearUTFChars(input: HelperStringUtils.FindTextBetween(
            pageText: companyPageText,
            textStart: "Indices:<dd>",
            textEnd: "</dd>"));
    }

    /// <summary>
    ///     Gets the currency
    /// </summary>
    /// <param name="pageText"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static string T2M_Currency(string pageText)
    {
        string currISO3 = "N/A";

        // Try to figure out if it has a "currency" or else get it from Year High
        string likelyCurrency = HelperStringUtils.FindTextBetween(
            pageText: pageText,
            textStart: "Currency:",
            textEnd: "</div>");
        if (likelyCurrency == string.Empty)
        {
            string currencyInYearHigh = HelperStringUtils.FindTextBetween(
                pageText: pageText,
                textStart: "Year high:",
                textEnd: "</div>");
            if (!string.IsNullOrWhiteSpace(value: currencyInYearHigh))
            {
                if (currencyInYearHigh.EndsWith(value: "p"))
                {
                    currISO3 = "GBX";
                }
                else if (currencyInYearHigh.StartsWith(value: "£") ||
                         currencyInYearHigh.StartsWith(value: "&pound;"))
                {
                    currISO3 = "GBP";
                }
                else if (currencyInYearHigh.StartsWith(value: "$"))
                {
                    currISO3 = "USD";
                }
                else if (currencyInYearHigh.StartsWith(value: "&euro;"))
                {
                    currISO3 = "EUR";
                }
                else if (currencyInYearHigh.StartsWith(value: "A$"))
                {
                    currISO3 = "AUD";
                }
                else
                {
                    if (currencyInYearHigh.Contains(value: "n/a"))
                        currISO3 = "N/A";
                    else
                        throw new Exception(message: $"Invalid currency: {currencyInYearHigh}.");
                }
            }
        }
        else
        {
            currISO3 = likelyCurrency;
        }

        return currISO3;
    }

    /// <summary>
    ///     Gets the currency sign
    /// </summary>
    /// <param name="currISO3"></param>
    /// <returns></returns>
    public static string T2M_CurrencySign(string currISO3)
    {
        return currISO3 switch
        {
            "USD" => "$",
            "EUR" => "&euro;",
            "GBX" => "p",
            "GBP" => "&pound;",
            "AUD" => "A$",
            _ => currISO3
        };
    }

    /// <summary>
    ///     Gets the GBP eqv from the daily API call
    /// </summary>
    /// <param name="currISO3"></param>
    /// <returns></returns>
    public static double T2M_GBPEquivalent(string currISO3)
    {
        return currISO3 != "N/A"
            ? currISO3 switch
            {
                "GBP" => 1,
                "GBX" => 0.01,
                _ => FrmMainApp.FxCurrencies.FindInverseRateByCode(CurrencyCode: currISO3) ?? 1
            }
            : 1;
    }

    /// <summary>
    ///     Gets the latest open price. In some case that's an N/A espc when weekend so we can pull the last-close price.
    /// </summary>
    /// <param name="pageText"></param>
    /// <returns></returns>
    public static double T2M_Open_price(string pageText)
    {
        string openStr = HelperStringUtils.FindTextBetween(
            pageText: pageText,
            textStart: "Open:",
            textEnd: "</div>");
        if (openStr == string.Empty ||
            openStr.ToLower() == "n/a")
            openStr = HelperStringUtils.FindTextBetween(
                pageText: pageText,
                textStart: "Previous close:",
                textEnd: "</div>");


        _ = double.TryParse(s: Regex.Replace(input: openStr, pattern: "[^. 0-9]", replacement: ""),
            result: out double openPrice);
        return openPrice;
    }

    /// <summary>
    ///     Gets the dividend yield value. Returns 0 if it looks fishy.
    /// </summary>
    /// <param name="pageText"></param>
    /// <returns></returns>
    public static string T2M_Dividend_yield(string pageText)
    {
        string divYieldStr = HelperStringUtils.FindTextBetween(
            pageText: pageText,
            textStart: "Dividend yield:",
            textEnd: "</div>");

        _ = double.TryParse(s: Regex.Replace(input: divYieldStr, pattern: "[^. 0-9]", replacement: ""),
            result: out double divYieldDbl);

        if (string.IsNullOrWhiteSpace(value: divYieldStr) ||
            divYieldStr.Contains(value: "n/a")) divYieldDbl = 0;

        return $"{divYieldDbl}%";
    }

    /// <summary>
    ///     Gets the year low
    /// </summary>
    /// <param name="pageText"></param>
    /// <param name="openVal"></param>
    /// <returns>The Min(openVal, year_low) value</returns>
    public static double T2M_Year_low(string pageText, double openVal)
    {
        string yearLowStr = HelperStringUtils.FindTextBetween(
            pageText: pageText,
            textStart: "Year low:",
            textEnd: "</div>");
        _ = double.TryParse(s: Regex.Replace(input: yearLowStr, pattern: "[^. 0-9]", replacement: ""),
            result: out double yearLowPrice);

        try
        {
            return Math.Min(val1: openVal, val2: yearLowPrice);
        }
        catch (Exception e)
        {
            return 0;
        }
    }

    /// <summary>
    ///     Gets the year high
    /// </summary>
    /// <param name="pageText"></param>
    /// <param name="openVal"></param>
    /// <returns>The Max(openVal, year_high) value</returns>
    public static double T2M_Year_high(string pageText, double openVal)
    {
        string yearLowStr = HelperStringUtils.FindTextBetween(
            pageText: pageText,
            textStart: "Year high:",
            textEnd: "</div>");
        _ = double.TryParse(s: Regex.Replace(input: yearLowStr, pattern: "[^. 0-9]", replacement: ""),
            result: out double yearHighPrice);

        try
        {
            return Math.Max(val1: openVal, val2: yearHighPrice);
        }
        catch (Exception e)
        {
            return 0;
        }
    }

    //public static double T2M_Level_pct(string pageText)
    //{
    //}

    /// <summary>
    ///     Gets the PE Ratio
    /// </summary>
    /// <param name="pageText"></param>
    /// <param name="currSign"></param>
    /// <returns></returns>
    public static double T2M_PE_ratio(string pageText, string currSign)
    {
        string peRatioStr = HelperStringUtils.FindTextBetween(
            pageText: pageText,
            textStart: "P/E ratio:",
            textEnd: "</div>");

        _ = double.TryParse(s: Regex.Replace(input: peRatioStr, pattern: "[^. 0-9]", replacement: ""),
            result: out double peRatio);
        return peRatio;
    }

    /// <summary>
    ///     Gets the market cap. This is stored as a string w/ units (eg "million") on the website so we try to convert it to
    ///     numbers.
    /// </summary>
    /// <param name="pageText"></param>
    /// <param name="currISO3"></param>
    /// <param name="currSign"></param>
    /// <returns></returns>
    /// <exception cref="Exception">Warn if encountered unknown value</exception>
    public static double T2M_Market_capitalisation(string pageText, string currISO3, string currSign)
    {
        int currPowerAdd = currISO3 == "GBX" ? 2 : 0;
        string marketCapStr = HelperStringUtils.FindTextBetween(
                                                    pageText: pageText,
                                                    textStart: "Market capitalisation:",
                                                    textEnd: "</div>").Replace(oldValue: ":", newValue: "")
                                               .Replace(oldValue: currISO3, newValue: "")
                                               .Replace(oldValue: currSign, newValue: "")
                                               .Trim(); // not sure why but we appear to need that replace (":") here.

        _ = double.TryParse(s: Regex.Replace(input: marketCapStr, pattern: "[^. 0-9]", replacement: ""),
            result: out double marketCap);
        if (
            !string.IsNullOrWhiteSpace(value: marketCapStr) &&
            marketCapStr.ToLower() != "n/a" &&
            marketCapStr.Length <
            100 // some trusts can have a "Market Capitalisation" text block but it's basically wrong.
        )
        {
            if (marketCapStr.Contains(value: "trillion") ||
                marketCapStr.Contains(value: "tn"))
            {
                marketCap *= Math.Pow(x: 10, y: 12 + currPowerAdd);
            }
            else if (marketCapStr.Contains(value: "billion") ||
                     marketCapStr.Contains(value: "bn"))
            {
                marketCap *= Math.Pow(x: 10, y: 9 + currPowerAdd);
            }
            else if (marketCapStr.Contains(value: "million") ||
                     marketCapStr.Contains(value: "mn"))
            {
                marketCap *= Math.Pow(x: 10, y: 6 + currPowerAdd);
            }
            else if (marketCapStr.Contains(value: ",") ||
                     marketCap < 1000)
            {
                // nothing. 
            }
            else
            {
                if (currISO3 != "N/A") throw new Exception(message: $"Error parsing marketcap - currency: {currISO3}");
            }
        }

        return marketCap;
    }

    /// <summary>
    ///     Gets the Volume
    /// </summary>
    /// <param name="pageText"></param>
    /// <returns></returns>
    public static double T2M_Volume(string pageText)
    {
        string openStr = HelperStringUtils.FindTextBetween(
            pageText: pageText,
            textStart: "Volume:",
            textEnd: "</div>");

        _ = double.TryParse(s: Regex.Replace(input: openStr, pattern: "[^. 0-9]", replacement: ""),
            result: out double openPrice);
        return openPrice;
    }
}