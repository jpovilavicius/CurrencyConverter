using CurrencyConverter.Domain.Models;
using CurrencyConverter.Domain.Models.Requests;
using CurrencyConverter.Domain.Services.Interfaces;
using CurrencyConverter.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyConverter.Domain.Services
{
    public class CalculationService : ICalculationService
    {
        private readonly IFileService _fileService;
        private readonly IOutputService _outputService;
        private readonly IExchangeRateService _exchangeRateService;

        public CalculationService(
            IFileService fileService,
            IOutputService outputService,
            IExchangeRateService exchangeRateService)
        {
            _fileService = fileService;
            _outputService = outputService;
            _exchangeRateService = exchangeRateService;
        }

        public async Task Calculate(string request)
        {
            try
            {
                var result = await GetResult(request);
                _outputService.Print(result);
            }
            catch (Exception e)
            {
                _outputService.Print($"Error occured while calculating rates. Error: {e.Message}");
            }
        }

        public async Task<string> GetResult(string request)
        {
            var fileResponse = await _fileService.ReadAsync();

            if (fileResponse.StatusCode == StatusCode.ServerError)
            {
                return fileResponse.Message;
            }

            var exchangeRequest = Mapper.MapToExchangeRateRequest(request);
            var requiredCurrencies = new List<string>
            {
                exchangeRequest.CurrencyFrom,
                exchangeRequest.CurrencyTo
            };

            var exchangeRates = _exchangeRateService.GetExchangeRates(fileResponse, requiredCurrencies);

            if (exchangeRates.Any(x => x.Error.HasError))
            {
                var error = exchangeRates.First(x => x.Error.HasError).Error;
                return error.Message;
            }

            var rate = GetCurrencyRate(exchangeRates, exchangeRequest);
            return rate.ToString(CultureInfo.InvariantCulture);
        }

        private decimal GetCurrencyRate(List<ExchangeRate> rates, ExchangeRateRequest request)
        {
            var rateFrom = rates.First(rate => rate.ISO == request.CurrencyFrom);
            var rateTo = rates.First(rate => rate.ISO == request.CurrencyTo);
            var result = rateFrom.Amount / rateTo.Amount * request.Amount;
            return result;
        }
    }
}
