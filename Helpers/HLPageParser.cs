using HtmlAgilityPack;
using System.Text.Json.Nodes;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

public static class HlPageParser
{
    // HL internal product IDs for ISA variants:
    // 22 = Stocks & Shares ISA, 25 = Lifetime ISA, 26 = Junior ISA
    private static readonly int[] IsaProductIds = { 22, 25, 26 };

    /// <summary>
    /// Gets key financial metrics and ISA tradability from the main factsheet HTML content into a Dictionary.
    /// </summary>
    public static Dictionary<string, string> ReturnPageText(string htmlContent)
    {
        Dictionary<string, string> result = [];

        HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
        doc.LoadHtml(htmlContent);

        HtmlNode? scriptNode = doc.DocumentNode.SelectSingleNode("//script[@id='__NEXT_DATA__']");
        if (scriptNode == null)
            return result;

        JsonNode? jsonRoot = JsonNode.Parse(scriptNode.InnerText);
        JsonNode? pageProps = jsonRoot?["props"]?["pageProps"];
        if (pageProps == null)
            return result;

        JsonNode? details = pageProps["investmentDetails"];
        JsonNode? metrics = pageProps["keyMetrics"];
        JsonNode? instrumentInfo = pageProps["instrumentInfoOverview"];

        // 1. Basic details & price metrics
        result["Name"] = GetJsonValue(details?["name"]);
        result["EPIC"] = GetJsonValue(details?["epicCode"]) ?? GetJsonValue(details?["epic"]);
        result["SEDOL"] = GetJsonValue(details?["sedol"]);
        result["ISIN"] = GetJsonValue(details?["isin"]);

        result["Description"] = GetJsonValue(details?["description"]);

        result["Currency"] = GetJsonValue(instrumentInfo?["keyFacts"]?["currency"]) ?? GetJsonValue(details?["open"]?["currency"]) ?? GetJsonValue(details?["close"]?["currency"]);

        result["OpenPrice"] = GetJsonValue(metrics?["open"]) ?? GetJsonValue(details?["open"]);

        result["PreviousClose"] = GetJsonValue(metrics?["previousClose"]) ?? GetJsonValue(details?["close"]);

        result["YearLow"] = GetJsonValue(metrics?["year_low"]) ?? GetJsonValue(details?["year_low"]) ?? GetJsonValue(instrumentInfo?["keyMetrics"]?["yearLow"]);

        result["YearHigh"] = GetJsonValue(metrics?["year_high"]) ?? GetJsonValue(details?["year_high"]) ?? GetJsonValue(instrumentInfo?["keyMetrics"]?["yearHigh"]);

        result["VolumeTraded"] = GetJsonValue(metrics?["volume_traded"]) ?? GetJsonValue(details?["volume_traded"]) ?? GetJsonValue(instrumentInfo?["keyMetrics"]?["volume"]);

        result["DividendYield"] = GetJsonValue(metrics?["dividend_yield"]) ?? GetJsonValue(details?["dividend_yield"]) ?? GetJsonValue(instrumentInfo?["keyMetrics"]?["dividendYield"]);

        result["PERatio"] = GetJsonValue(metrics?["pERatio"]) ?? GetJsonValue(details?["pERatio"]) ?? GetJsonValue(instrumentInfo?["keyMetrics"]?["pERatio"]);

        result["MarketCap"] = GetJsonValue(metrics?["marketCap"]) ?? GetJsonValue(details?["market_cap"]) ?? GetJsonValue(instrumentInfo?["keyMetrics"]?["marketCap"]);

        result["Type"] = GetJsonValue(details?["type"]);

        string sector = GetJsonValue(instrumentInfo?["overview"]?["keyFacts"]?["sector"]);

        if (string.IsNullOrEmpty(sector))
        {
            JsonNode? topSector = instrumentInfo?["assetBreakdown"]?["topTenDetails"]?["sectors"]?[0];
            if (topSector != null)
            {
                sector = $"{topSector["name"]} ({topSector["percentage"]}%)";
            }
        }

        result["Sector"] = sector;

        result["Exchange"] = GetJsonValue(instrumentInfo?["overview"]?["keyFacts"]?["exchange"]);
        result["Country"] = GetJsonValue(instrumentInfo?["overview"]?["keyFacts"]?["location"]);

        bool isIsaTradeable = false;

        JsonArray? applicableProducts = details?["applicableProducts"]?.AsArray();
        if (applicableProducts != null)
        {
            isIsaTradeable = applicableProducts.Any(node =>
                node != null && int.TryParse(node.ToString(), out int id) && IsaProductIds.Contains(id));
        }

        if (!isIsaTradeable)
        {
            HtmlNode? isaAccountNode = doc.DocumentNode.SelectSingleNode(
                "//a[contains(@href, '/investment-services/isa') or contains(@data-search, 'isa')][.//use[@href='#circle-check']]"
            );
            isIsaTradeable = isaAccountNode != null;
        }

        result["IsIsaTradeable"] = isIsaTradeable ? "True" : "False";

        return result;
    }


    /// <summary>
    /// Gets corporate/fund details from the company information page HTML content into a Dictionary.
    /// </summary>
    public static Dictionary<string, string> ReturnCompanyPageText(string htmlContent)
    {
        Dictionary<string, string> result = [];

        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(htmlContent);

        HtmlNode? scriptNode = doc.DocumentNode.SelectSingleNode("//script[@id='__NEXT_DATA__']");
        if (scriptNode == null)
        {
            result["Error"] = "Crap Data";
            return result;
        }

        try
        {
            JsonNode? jsonRoot = JsonNode.Parse(scriptNode.InnerText);
            JsonNode? pageProps = jsonRoot?["props"]?["pageProps"];
            if (pageProps == null)
            {
                result["Error"] = "Crap Data";
                return result;
            }

            JsonNode? details = pageProps["investmentDetails"];
            JsonNode? instrumentInfo = pageProps["instrumentInfo"];
            JsonNode? keyFacts = instrumentInfo?["overview"]?["keyFacts"];
            JsonNode? assetBreakdown = instrumentInfo?["assetBreakdown"]?["topTenDetails"];

            result["CompanyTitle"] = GetJsonValue(details?["title"]) ?? "N/A";
            result["EPIC"] = GetJsonValue(details?["epic"]) ?? GetJsonValue(details?["epicCode"]) ?? "N/A";
            result["ISIN"] = GetJsonValue(details?["isin"]) ?? GetJsonValue(keyFacts?["isin"]) ?? "N/A";
            result["InstrumentType"] = GetJsonValue(details?["type"]) ?? "N/A";

            // Extract Top Holdings if available
            JsonArray? holdingsNode = assetBreakdown?["holdings"]?.AsArray();
            if (holdingsNode != null && holdingsNode.Count > 0)
            {
                result["TopHoldings"] = string.Join(", ", holdingsNode.Select(h => $"{h?["name"]} ({h?["percentage"]}%)"));
            }
            else
            {
                result["TopHoldings"] = "None";
            }

            // Extract Sector Exposure if available
            JsonArray? sectorsNode = assetBreakdown?["sectors"]?.AsArray();
            if (sectorsNode != null && sectorsNode.Count > 0)
            {
                result["Sectors"] = string.Join(", ", sectorsNode.Select(s => $"{s?["name"]} ({s?["percentage"]}%)"));
            }
            else
            {
                result["Sectors"] = "None";
            }
        }
        catch (Exception)
        {
            result.Clear();
            result["Error"] = "Crap Data";
        }

        return result;
    }

    /// <summary>
    /// Converts a JSON value to string, returns string.Empty if none found.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    private static string? GetJsonValue(JsonNode? node)
    {
        if (node == null)
            return null;

        // If it's a JSON object (e.g., { "value": 4417.5 }), pull ["value"]
        if (node is JsonObject obj)
        {
            string? objectVal = obj["value"]?.ToString();
            if (string.IsNullOrWhiteSpace(objectVal) || objectVal == "null")
            {
                return null;
            }

            return obj["value"]?.ToString();
        }

        // If it's a primitive value (e.g., "2483"), return string directly
        if (string.IsNullOrWhiteSpace(node.ToString()))
        {
            return null;
        }

        return node.ToString();
    }
}

