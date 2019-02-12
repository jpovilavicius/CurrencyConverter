using Microsoft.Extensions.Configuration;

namespace CurrencyConverter.Domain.Configuration
{
    public static class AppConfiguration
    {
        public static IConfiguration Configuration { get; set; }
        public static string FileName => Configuration["Data:FileName"];
        public static string DefaultCurrency => Configuration["Data:DefaultCurrency"];
        public static decimal DefaultCurrencyRatio => decimal.Parse(Configuration["Data:DefaultCurrencyRatio"]);
    }
}
