namespace HLWebScraper.Net.Model;

internal class FXCurrencyCollection : List<FXCurrency>
{
    public FXCurrency FindFXCurrencyByCode(string CurrencyCode)
    {
        return this.FirstOrDefault(predicate: item => item.Code == CurrencyCode);
    }

    public double? FindInverseRateByCode(string CurrencyCode)
    {
        return this.FirstOrDefault(predicate: item => item.Code == CurrencyCode).InverseRate;
    }
}