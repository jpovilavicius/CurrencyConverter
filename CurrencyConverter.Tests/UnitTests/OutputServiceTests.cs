using CurrencyConverter.Domain.Services;
using CurrencyConverter.Domain.Services.Interfaces;
using System;
using Xunit;

namespace CurrencyConverter.Tests.UnitTests
{
    public class OutputServiceTests
    {
        private readonly IOutputService _outputService;

        public OutputServiceTests()
        {
            _outputService = new OutputService();
        }

        [Fact]
        public void Should_PrintResult()
        {
            Exception error = null;

            try
            {
                _outputService.Print("");
            }
            catch (Exception e)
            {
                error = e;

            }

            Assert.Null(error);
        }
    }
}
