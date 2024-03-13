using Newtonsoft.Json;

namespace HLWebScraper.Net.Model;

internal class FXCurrency
{
    [JsonProperty(propertyName: "code")] public string Code { get; set; }

    [JsonProperty(propertyName: "alphaCode")]
    public string AlphaCode { get; set; }

    [JsonProperty(propertyName: "numericCode")]
    public string NumericCode { get; set; }

    [JsonProperty(propertyName: "name")] public string Name { get; set; }

    [JsonProperty(propertyName: "rate")] public double Rate { get; set; }

    [JsonProperty(propertyName: "date")] public string Date { get; set; }

    [JsonProperty(propertyName: "inverseRate")]
    public double InverseRate { get; set; }
}