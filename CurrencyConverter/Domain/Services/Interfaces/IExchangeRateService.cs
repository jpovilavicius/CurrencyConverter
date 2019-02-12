using CurrencyConverter.Domain.Models;
using CurrencyConverter.Domain.Models.Responses;
using System.Collections.Generic;

namespace CurrencyConverter.Domain.Services.Interfaces
{
    public interface IExchangeRateService
    {
        List<ExchangeRate> GetExchangeRates(FileResponse response, List<string> requiredCurrencies);
    }
}
