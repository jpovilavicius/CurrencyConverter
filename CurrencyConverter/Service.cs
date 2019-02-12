using CurrencyConverter.Domain.Services;
using CurrencyConverter.Domain.Services.Interfaces;
using LightInject;
using System.Threading.Tasks;

namespace CurrencyConverter
{
    public class Service
    {
        public async Task Start(string request)
        {
            var container = new ServiceContainer();
            RegisterService(container);

            var calculationService = container.GetInstance<ICalculationService>();
            await calculationService.Calculate(request);
        }

        private void RegisterService(IServiceRegistry container)
        {
            container.Register<IFileService, FileService>();
            container.Register<IExchangeRateService, ExchangeRateService>();
            container.Register<ICalculationService, CalculationService>();
            container.Register<IOutputService, OutputService>();
        }
    }
}
