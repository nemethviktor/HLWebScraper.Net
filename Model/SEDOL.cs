namespace HLWebScraper.Net.Model;

internal class SEDOL
{
    public string URL { get; set; }
    public string Name { get; set; }
    public bool Is_ISA_Compatible { get; set; }
    public string Ticker { get; set; }
    public string SEDOL_ID { get; set; }
    public string Sector { get; set; }
    public string ETF_Type { get; set; }
    public string Top10_Components { get; set; }
    public string Exchange { get; set; }
    public string Country { get; set; }
    public string Indices { get; set; }
    public string Currency { get; set; }
    public double Open_price { get; set; }
    public double Year_low { get; set; }
    public double Year_high { get; set; }
    public string Dividend_yield { get; set; }
    public double PE_ratio { get; set; }
    public double Market_capitalisation { get; set; }
    public double Volume { get; set; }
    public double GBP_Open { get; set; }
    public double GBP_Year_low { get; set; }
    public double GBP_Year_high { get; set; }
    public double GBP_Market_capitalisation { get; set; }
}