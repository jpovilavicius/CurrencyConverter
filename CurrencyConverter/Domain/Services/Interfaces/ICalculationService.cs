using System.Threading.Tasks;

namespace CurrencyConverter.Domain.Services.Interfaces
{
    public interface ICalculationService
    {
        Task Calculate(string request);
    }
}
