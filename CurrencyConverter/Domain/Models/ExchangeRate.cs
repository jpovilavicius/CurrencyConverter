using System.Diagnostics.CodeAnalysis;

namespace CurrencyConverter.Domain.Models
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ExchangeRate
    {
        public string Currency { get; set; }
        public string ISO { get; set; }
        public decimal Amount { get; set; }
        public Error Error { get; set; }
    }
}
