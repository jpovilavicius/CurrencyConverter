using CurrencyConverter.Domain.Configuration;
using CurrencyConverter.Domain.Models;
using CurrencyConverter.Domain.Models.Responses;
using CurrencyConverter.Domain.Services;
using CurrencyConverter.Domain.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CurrencyConverter.Tests.UnitTests
{
    public class CalculationServiceTests
    {
        private readonly Mock<IFileService> _fileService = new Mock<IFileService>();
        private readonly Mock<IExchangeRateService> _exchangeService = new Mock<IExchangeRateService>();
        private readonly Mock<IOutputService> _outputService = new Mock<IOutputService>();

        public CalculationServiceTests()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            AppConfiguration.Configuration = config;
        }

        [Fact]
        public async Task Should_Calculate()
        {
            const string request = "EUR/USD 1";
            var fileResponse = new FileResponse
            {
                StatusCode = StatusCode.Ok,
                Lines = new List<string>
                {
                    "Euro, EUR, 743.94",
                    "Amerikanske dollar, USD, 663.11"
                }
            };

            var exchangeRates = new List<ExchangeRate>
            {
                new ExchangeRate
                {
                    Currency = "Euro",
                    ISO = "EUR",
                    Amount = 743.94M / AppConfiguration.DefaultCurrencyRatio,
                    Error = new Error { HasError = false }
                },
                new ExchangeRate
                {
                    Currency = "Amerikanske dollar",
                    ISO = "USD",
                    Amount = 663.11M / AppConfiguration.DefaultCurrencyRatio,
                    Error = new Error { HasError = false }
                }
            };

            _fileService.Setup(x => x.ReadAsync()).Returns(Task.FromResult(fileResponse));
            _exchangeService.Setup(x => x.GetExchangeRates(It.IsAny<FileResponse>(), It.IsAny<List<string>>()))
                .Returns(exchangeRates);

            var service = new CalculationService(_fileService.Object, _outputService.Object, _exchangeService.Object);

            Exception exception = null;

            try
            {
                await service.Calculate(request).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                exception = e;
            }

            Assert.Null(exception);
        }

        [Fact]
        public async Task Should_GetResult()
        {
            const string request = "EUR/USD 1";
            var fileResponse = new FileResponse
            {
                StatusCode = StatusCode.Ok,
                Lines = new List<string>
                {
                    "Euro, EUR, 743.94",
                    "Amerikanske dollar, USD, 663.11"
                }
            };

            var exchangeRates = new List<ExchangeRate>
            {
                new ExchangeRate
                {
                    Currency = "Euro",
                    ISO = "EUR",
                    Amount = 743.94M / AppConfiguration.DefaultCurrencyRatio,
                    Error = new Error { HasError = false }
                },
                new ExchangeRate
                {
                    Currency = "Amerikanske dollar",
                    ISO = "USD",
                    Amount = 663.11M / AppConfiguration.DefaultCurrencyRatio,
                    Error = new Error { HasError = false }
                }
            };

            var amount = exchangeRates[0].Amount / exchangeRates[1].Amount * 1;

            _fileService.Setup(x => x.ReadAsync()).Returns(Task.FromResult(fileResponse));
            _exchangeService.Setup(x => x.GetExchangeRates(It.IsAny<FileResponse>(), It.IsAny<List<string>>()))
                .Returns(exchangeRates);

            var service = new CalculationService(_fileService.Object, _outputService.Object, _exchangeService.Object);
            var result = await service.GetResult(request).ConfigureAwait(false);

            Assert.Equal(amount.ToString(CultureInfo.InvariantCulture), result);
        }

        [Fact]
        public async Task ShouldShow_FileResponseErrorMessage()
        {
            const string request = "EUR/USD 1";
            var fileResponse = new FileResponse
            {
                StatusCode = StatusCode.ServerError,
                Message = "Error occured on ReadService.ReadAsync:"
            };

            var exchangeRates = new List<ExchangeRate>
            {
                new ExchangeRate
                {
                    Currency = "Euro",
                    ISO = "EUR",
                    Amount = 743.94M / AppConfiguration.DefaultCurrencyRatio,
                    Error = new Error { HasError = false }
                },
                new ExchangeRate
                {
                    Currency = "Amerikanske dollar",
                    ISO = "USD",
                    Amount = 663.11M / AppConfiguration.DefaultCurrencyRatio,
                    Error = new Error { HasError = false }
                }
            };

            _fileService.Setup(x => x.ReadAsync()).Returns(Task.FromResult(fileResponse));
            _exchangeService.Setup(x => x.GetExchangeRates(It.IsAny<FileResponse>(), It.IsAny<List<string>>()))
                .Returns(exchangeRates);

            var service = new CalculationService(_fileService.Object, _outputService.Object, _exchangeService.Object);
            var result = await service.GetResult(request).ConfigureAwait(false);

            Assert.Equal(fileResponse.Message, result);
        }

        [Fact]
        public async Task ShouldShow_ExchangeRateErrorMessage()
        {
            const string request = "EUR/USD 1";
            var fileResponse = new FileResponse
            {
                StatusCode = StatusCode.Ok,
                Lines = new List<string>
                {
                    "Euro, EUR, 743.94",
                    "Amerikanske dollar, USD, 663.11"
                }
            };

            var exchangeRates = new List<ExchangeRate>
            {
                new ExchangeRate
                {
                    Currency = "Euro",
                    ISO = "EUR",
                    Amount = 743.94M / AppConfiguration.DefaultCurrencyRatio,
                    Error = new Error { HasError = false }
                },
                new ExchangeRate
                {
                    Currency = "Amerikanske dollar",
                    ISO = "USD",
                    Amount = 663.11M / AppConfiguration.DefaultCurrencyRatio,
                    Error = new Error { HasError = true, Message = "Error"}
                }
            };

            var expectedResult = exchangeRates.First(x => x.Error.HasError).Error.Message;

            _fileService.Setup(x => x.ReadAsync()).Returns(Task.FromResult(fileResponse));
            _exchangeService.Setup(x => x.GetExchangeRates(It.IsAny<FileResponse>(), It.IsAny<List<string>>()))
                .Returns(exchangeRates);

            var service = new CalculationService(_fileService.Object, _outputService.Object, _exchangeService.Object);
            var result = await service.GetResult(request).ConfigureAwait(false);

            Assert.Equal(expectedResult, result);
        }
    }
}
