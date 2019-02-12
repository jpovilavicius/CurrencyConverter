using CurrencyConverter.Domain.Configuration;
using CurrencyConverter.Domain.Models;
using CurrencyConverter.Domain.Models.Responses;
using CurrencyConverter.Domain.Services;
using CurrencyConverter.Domain.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Xunit;

namespace CurrencyConverter.Tests.UnitTests
{
    public class ExchangeRateServiceTests
    {
        private readonly IExchangeRateService _exchangeRateService;

        public ExchangeRateServiceTests()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            AppConfiguration.Configuration = config;

            _exchangeRateService = new ExchangeRateService();
        }

        [Fact]
        public void Should_GetExchangeRates()
        {
            var requiredCurrencies = new List<string> { "EUR", "USD" };
            var fileResponse = new FileResponse
            {
                StatusCode = StatusCode.Ok,
                Lines = new List<string>
                {
                    "Euro, EUR, 743.94",
                    "Amerikanske dollar, USD, 663.11"
                }
            };

            var result = _exchangeRateService.GetExchangeRates(fileResponse, requiredCurrencies);
            Assert.Equal("EUR", result[0].ISO);
            Assert.Equal("Euro", result[0].Currency);
            Assert.Equal(743.94M / AppConfiguration.DefaultCurrencyRatio, result[0].Amount);
            Assert.False(result[0].Error.HasError);

            Assert.Equal("USD", result[1].ISO);
            Assert.Equal("Amerikanske dollar", result[1].Currency);
            Assert.Equal(663.11M / AppConfiguration.DefaultCurrencyRatio, result[1].Amount);
            Assert.False(result[1].Error.HasError);
        }

        [Fact]
        public void Should_GetExchangeRates_WithDefaultCurrency()
        {
            var defaultCurrency = AppConfiguration.DefaultCurrency;
            var requiredCurrencies = new List<string> { "EUR", defaultCurrency };
            var fileResponse = new FileResponse
            {
                StatusCode = StatusCode.Ok,
                Lines = new List<string>
                {
                    "Euro, EUR, 743.94",
                    "Amerikanske dollar, USD, 663.11"
                }
            };

            var result = _exchangeRateService.GetExchangeRates(fileResponse, requiredCurrencies);
            Assert.Equal("EUR", result[0].ISO);
            Assert.Equal("Euro", result[0].Currency);
            Assert.Equal(743.94M / AppConfiguration.DefaultCurrencyRatio, result[0].Amount);
            Assert.False(result[0].Error.HasError);

            Assert.Equal(defaultCurrency, result[1].ISO);
            Assert.Equal(defaultCurrency, result[1].Currency);
            Assert.Equal(1, result[1].Amount);
            Assert.False(result[1].Error.HasError);
        }

        [Fact]
        public void ShouldFailTo_GetExchangeRates_WhenWrongCurrency()
        {
            var requiredCurrencies = new List<string> { "EUR", "NOK" };
            var fileResponse = new FileResponse
            {
                StatusCode = StatusCode.Ok,
                Lines = new List<string>
                {
                    "Euro, EUR, 743.94",
                    "Amerikanske dollar, USD, 663.11"
                }
            };

            var result = _exchangeRateService.GetExchangeRates(fileResponse, requiredCurrencies);
            Assert.Equal("EUR", result[0].ISO);
            Assert.Equal("Euro", result[0].Currency);
            Assert.Equal(743.94M / AppConfiguration.DefaultCurrencyRatio, result[0].Amount);
            Assert.False(result[0].Error.HasError);

            Assert.True(result[1].Error.HasError);
            Assert.Equal("Currency - NOK is not valid.", result[1].Error.Message);
        }

        [Fact]
        public void ShouldFailTo_GetExchangeRates_WhenISONotExist()
        {
            var requiredCurrencies = new List<string> { "EUR", "USDD" };
            var fileResponse = new FileResponse
            {
                StatusCode = StatusCode.Ok,
                Lines = new List<string>
                {
                    "Euro, EUR, 743.94",
                    "Amerikanske dollar, USD, amount"
                }
            };

            var result = _exchangeRateService.GetExchangeRates(fileResponse, requiredCurrencies);
            Assert.Equal("EUR", result[0].ISO);
            Assert.Equal("Euro", result[0].Currency);
            Assert.Equal(743.94M / AppConfiguration.DefaultCurrencyRatio, result[0].Amount);
            Assert.False(result[0].Error.HasError);

            Assert.True(result[1].Error.HasError);
            Assert.Equal("Currency - USDD is not valid.", result[1].Error.Message);
        }

        [Fact]
        public void ShouldFailTo_GetExchangeRates_WhenWrongAmount()
        {
            var requiredCurrencies = new List<string> { "EUR", "USD" };
            var fileResponse = new FileResponse
            {
                StatusCode = StatusCode.Ok,
                Lines = new List<string>
                {
                    "Euro, EUR, 743.94",
                    "Amerikanske dollar, USD, amount"
                }
            };

            var result = _exchangeRateService.GetExchangeRates(fileResponse, requiredCurrencies);
            Assert.Equal("EUR", result[0].ISO);
            Assert.Equal("Euro", result[0].Currency);
            Assert.Equal(743.94M / AppConfiguration.DefaultCurrencyRatio, result[0].Amount);
            Assert.False(result[0].Error.HasError);

            Assert.True(result[1].Error.HasError);
            Assert.Equal("Currency - USD is not valid. Amount is not numeric.", result[1].Error.Message);
        }
    }
}
