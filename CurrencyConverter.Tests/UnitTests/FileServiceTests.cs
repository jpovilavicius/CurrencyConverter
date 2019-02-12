using CurrencyConverter.Domain.Models;
using CurrencyConverter.Domain.Models.Responses;
using CurrencyConverter.Domain.Services.Interfaces;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace CurrencyConverter.Tests.UnitTests
{
    public class FileServiceTests
    {
        [Fact]
        public async Task ShouldFailTo_ReadAsync()
        {
            var response = new FileResponse
            {
                Lines = null,
                StatusCode = StatusCode.ServerError
            };

            var service = new Mock<IFileService>();
            service.Setup(x => x.ReadAsync()).Returns(Task.FromResult(response));

            var result = await service.Object.ReadAsync().ConfigureAwait(false);
            Assert.Null(result.Lines);
            Assert.Equal(StatusCode.ServerError, result.StatusCode);
        }
    }
}
