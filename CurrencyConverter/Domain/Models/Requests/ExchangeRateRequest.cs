namespace CurrencyConverter.Domain.Models.Requests
{
    public class ExchangeRateRequest
    {
        public string CurrencyFrom { get; set; }
        public string CurrencyTo { get; set; }
        public decimal Amount { get; set; }
    }
}
