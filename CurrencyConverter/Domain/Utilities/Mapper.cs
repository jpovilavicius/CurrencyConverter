using CurrencyConverter.Domain.Models.Requests;

namespace CurrencyConverter.Domain.Utilities
{
    public static class Mapper
    {
        public static ExchangeRateRequest MapToExchangeRateRequest(string request)
        {
            var result = new ExchangeRateRequest();
            var parameters = request.Split(" ");

            if (parameters.Length <= 1)
            {
                return result;
            }

            var currency = parameters[0].Split("/");
            decimal.TryParse(parameters[1], out var amount);

            result.CurrencyFrom = currency[0];
            result.CurrencyTo = currency[1];
            result.Amount = amount;

            return result;
        }
    }
}
