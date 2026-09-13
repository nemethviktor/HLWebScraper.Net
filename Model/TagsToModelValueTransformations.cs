using HLWebScraper.Net.Helpers;
using System.Text.RegularExpressions;

namespace HLWebScraper.Net.Model;

internal static class TagsToModelValueTransformations
{

    /// <summary>
    ///     Somewhat arbitrarily attempts to figure if something is stuff like Gilts or an ETF or not. If likely neither then
    ///     we try to pull the Sector info from teh company-info page.
    ///     Items that have a non-zero market cap are definitely not ETFs
    /// </summary>
    /// <param name="dataInContentHashtableSector"></param>
    /// <param name="securityNameLowerCase"></param>
    /// <param name="ticker"></param>
    /// <param name="marketCapOverZero"></param>
    /// <returns></returns>
    public static string T2M_Sector(string dataInContentHashtableSector, string securityNameLowerCase, string ticker,
        bool marketCapOverZero)
    {
        securityNameLowerCase = securityNameLowerCase.ToLower();
        string sector = "Unspecified";

        // Items that have a non - zero market cap are definitely not ETFs
        if (!marketCapOverZero)
        {
            List<string> invalidatingContainerList = [" ord ", " ordinary ", " npv", "stk", ".0", ".1"];
            if (!invalidatingContainerList.Any(predicate: container =>
                    securityNameLowerCase.Contains(value: container)))
            {
                List<string> etf1xContainerList =
                [
                    "daily ", " daily", "invesco", "etf ", " etf", "fund ", " fund", "ucits", "msci", "accum", "etc",
                    "ishares", "growth", "lyxor", "commodity", "index", "wisdomtree", "wisdom tree", "gold bu",
                    "xtrackers", "multi unit", "xbt provider"
                ];
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
                    List<string> ignoreContainsList =
                    [
                        "short term",
                        "short-term",
                        "matur",
                        "ultra",
                        "duration"
                    ];
                    if (!ignoreContainsList.Any(predicate: container => sector.Contains(value: container)))
                        sector = sector.Replace(oldValue: "ETF ", newValue: "ETF -");
                }
            }
        }

        // try and pull from website. this might be a logic first step but i'd like to qualify etfs separately.
        string likelySector = string.Empty;
        if (sector == "Unspecified")
            likelySector = dataInContentHashtableSector;

        return HelperStringUtils.ClearUTFChars(input: string.IsNullOrWhiteSpace(value: likelySector)
            ? sector.Replace(oldValue: "&", newValue: "and").Replace(oldValue: "--", newValue: "-")
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
            if (name.Contains(value: etfType.Keyword, comparisonType: StringComparison.CurrentCultureIgnoreCase))
                return etfType.ETF_Type;

        return "## Not classified";
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
    /// <param name="openStr"></param>
    /// <returns></returns>
    public static double T2M_Open_price(string openStr)
    {
        _ = double.TryParse(s: Regex.Replace(input: openStr, pattern: "[^. 0-9]", replacement: ""),
            result: out double openPrice);
        return openPrice;
    }

    /// <summary>
    ///     Gets the dividend yield value. Returns 0 if it looks fishy.
    /// </summary>
    /// <param name="pageText"></param>
    /// <returns></returns>
    public static string T2M_Dividend_yield(string divYieldStr)
    {

        _ = double.TryParse(s: Regex.Replace(input: divYieldStr, pattern: "[^. 0-9]", replacement: ""),
            result: out double divYieldDbl);

        if (string.IsNullOrWhiteSpace(value: divYieldStr) ||
            divYieldStr.Contains(value: "n/a")) divYieldDbl = 0;

        return $"{divYieldDbl}%";
    }

    /// <summary>
    ///     Gets the year low
    /// </summary>
    /// <param name="yearLowStr"></param>
    /// <param name="openVal"></param>
    /// <returns>The Min(openVal, year_low) value</returns>
    public static double T2M_Year_low(string yearLowStr, double openVal)
    {
        _ = double.TryParse(s: Regex.Replace(input: yearLowStr, pattern: "[^. 0-9]", replacement: ""),
           result: out double yearLowPrice);

        try
        {
            return Math.Min(val1: openVal, val2: yearLowPrice);
        }
        catch (Exception)
        {
            return 0;
        }
    }

    /// <summary>
    ///     Gets the year high
    /// </summary>
    /// <param name="yearHighStr"></param>
    /// <param name="openVal"></param>
    /// <returns>The Max(openVal, year_high) value</returns>
    public static double T2M_Year_high(string yearHighStr, double openVal)
    {
        _ = double.TryParse(s: Regex.Replace(input: yearHighStr, pattern: "[^. 0-9]", replacement: ""),
            result: out double yearHighPrice);

        try
        {
            return Math.Max(val1: openVal, val2: yearHighPrice);
        }
        catch (Exception)
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
    /// <param name="peRatioStr"></param>
    /// <param name="currSign"></param>
    /// <returns></returns>
    public static double T2M_PE_ratio(string peRatioStr, string currSign)
    {
        _ = double.TryParse(s: Regex.Replace(input: peRatioStr, pattern: "[^. 0-9]", replacement: ""),
            result: out double peRatio);
        return peRatio;
    }

    /// <summary>
    /// Parses market cap strings (or raw numeric strings) into a double value.
    /// Converts GBX (pence) to GBP if needed.
    /// </summary>
    public static double T2M_Market_capitalisation(string marketCapStr, string currISO3, string currSign)
    {
        if (string.IsNullOrWhiteSpace(marketCapStr) ||
            marketCapStr.Equals("n/a", StringComparison.OrdinalIgnoreCase))
        {
            return 0;
        }

        // Clean out currency codes, symbols, and standard noise
        string cleanStr = marketCapStr
            .Replace(currISO3, "", StringComparison.OrdinalIgnoreCase)
            .Replace(currSign, "")
            .Replace(",", "")
            .Trim();

        // Extract pure numeric part (handles decimals like 53.4)
        string numericPart = Regex.Replace(cleanStr, @"[^0-9.]", "");
        if (!double.TryParse(numericPart, out double marketCap))
        {
            return 0;
        }

        string lowerStr = cleanStr.ToLowerInvariant();

        // 1. Handle textual multipliers (e.g. "53.4 million", "1.2 bn")
        if (lowerStr.Contains("trillion") || lowerStr.Contains("tn"))
        {
            marketCap *= 1e12; // 1,000,000,000,000
        }
        else if (lowerStr.Contains("billion") || lowerStr.Contains("bn"))
        {
            marketCap *= 1e9; // 1,000,000,000
        }
        else if (lowerStr.Contains("million") || lowerStr.Contains("mn"))
        {
            marketCap *= 1e6; // 1,000,000
        }

        // 2. Adjust GBX (Pence) to GBP (Pounds) if currency is pence
        if (string.Equals(currISO3, "GBX", StringComparison.OrdinalIgnoreCase))
        {
            // Optional: Divide by 100 if you want the market cap stored in Pounds (£) 
            // instead of Pence.
            // marketCap /= 100.0;
        }

        return marketCap;
    }

    /// <summary>
    ///     Gets the Volume
    /// </summary>
    /// <param name="volumeStr"></param>
    /// <returns></returns>
    public static double T2M_Volume(string volumeStr)
    {
        _ = double.TryParse(s: Regex.Replace(input: volumeStr, pattern: "[^. 0-9]", replacement: ""),
            result: out double volume);
        return volume;
    }

    /// <summary>
    ///     Gets the Top 10 Exposures where available
    /// </summary>
    /// <param name="pageText"></param>
    /// <returns></returns>
    public static string T2M_Top10_Exposures(string pageText)
    {
        string top10Exposures = string.Empty;
        string likelyExposures = HelperStringUtils.FindTextBetween(
            pageText: pageText,
            textStart: "<div id=\"top_10_exposures_data\">",
            textEnd: "</div>");
        if (likelyExposures.Contains(value: "No top ten information is available at this stage"))
            return top10Exposures;

        try
        {
            likelyExposures = HelperStringUtils.FindTextBetween(
                pageText: likelyExposures,
                textStart: "<tbody>",
                textEnd: "</tbody>");

            string[] lines = likelyExposures.Split(separator: new[] { '\r', '\n' },
                options: StringSplitOptions.RemoveEmptyEntries);


            foreach (string line in lines)
                // Check if the line contains table row data
                if (line.Contains(value: "<tr>"))
                {
                    // Extract text from the row (remove HTML tags)
                    string rowText = HelperStringUtils.ClearUTFChars(input: RemoveHtmlTags(line: line));

                    top10Exposures += rowText;
                }
        }
        catch
        {
            // nothing
        }

        return top10Exposures;

        string RemoveHtmlTags(string line)
        {
            // Remove HTML tags from the line using regular expression
            return Regex.Replace(input: line, pattern: "<.*?>", replacement: string.Empty).Trim();
        }
    }
}