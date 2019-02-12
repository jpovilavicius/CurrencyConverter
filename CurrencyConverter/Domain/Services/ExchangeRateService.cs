using CurrencyConverter.Domain.Configuration;
using CurrencyConverter.Domain.Models;
using CurrencyConverter.Domain.Models.Responses;
using CurrencyConverter.Domain.Services.Interfaces;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CurrencyConverter.Domain.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        public List<ExchangeRate> GetExchangeRates(FileResponse response, List<string> requiredCurrencies)
        {
            var rates = new List<ExchangeRate>();

            foreach (var currencyIso in requiredCurrencies)
            {
                var line = response.Lines.SingleOrDefault(x => x.Contains(currencyIso));
                var items = line?.Split(",");
                var rate = GetExchangeRate(currencyIso, items);
                rates.Add(rate);
            }

            return rates;
        }

        private ExchangeRate GetExchangeRate(string iso, string[] items)
        {
            var result = new ExchangeRate();
            var isDefaultCurrency = AppConfiguration.DefaultCurrency.Equals(iso);
            result.Error = ValidateExchangeRate(iso, items, isDefaultCurrency);

            if (result.Error.HasError)
            {
                return result;
            }


            SetExchangeRate(result, items, isDefaultCurrency);

            return result;
        }

        private Error ValidateExchangeRate(string iso, string[] items, bool isDefaultCurrency)
        {
            var error = new Error();

            if (items == null)
            {
                error.HasError = !isDefaultCurrency;
                error.Message = !isDefaultCurrency
                    ? $"Currency - {iso} is not valid."
                    : string.Empty;

                return error;
            }

            var isNumeric = decimal.TryParse(items[2].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out _);
            error.HasError = !isNumeric;
            error.Message = !isNumeric
                ? $"Currency - {iso} is not valid. Amount is not numeric."
                : string.Empty;

            return error;
        }

        private void SetExchangeRate(ExchangeRate rate, string[] items, bool isDefaultCurrency)
        {
            if (items != null)
            {
                var amount = decimal.Parse(items[2].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture);
                rate.Currency = items[0].Trim();
                rate.ISO = items[1].Trim();
                rate.Amount = amount / AppConfiguration.DefaultCurrencyRatio;
            }
            else if (isDefaultCurrency)
            {
                rate.Currency = AppConfiguration.DefaultCurrency;
                rate.ISO = AppConfiguration.DefaultCurrency;
                rate.Amount = 1;
            }
        }
    }
}
