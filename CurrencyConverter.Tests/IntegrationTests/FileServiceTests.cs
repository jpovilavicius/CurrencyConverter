using CurrencyConverter.Domain.Configuration;
using CurrencyConverter.Domain.Models;
using CurrencyConverter.Domain.Services;
using CurrencyConverter.Domain.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Xunit;

namespace CurrencyConverter.Tests.IntegrationTests
{
    public class FileServiceTests
    {
        private readonly IFileService _fileService;

        public FileServiceTests()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            AppConfiguration.Configuration = config;
            _fileService = new FileService();
        }

        [Fact]
        public async Task Should_ReadAsync()
        {
            var result = await _fileService.ReadAsync();
            Assert.NotEmpty(result.Lines);
            Assert.Equal(StatusCode.Ok, result.StatusCode);
        }
    }
}
