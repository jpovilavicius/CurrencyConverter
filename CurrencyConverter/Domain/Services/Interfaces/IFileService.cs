using CurrencyConverter.Domain.Models.Responses;
using System.Threading.Tasks;

namespace CurrencyConverter.Domain.Services.Interfaces
{
    public interface IFileService
    {
        Task<FileResponse> ReadAsync();
    }
}
