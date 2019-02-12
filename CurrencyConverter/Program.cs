using CurrencyConverter.Domain.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CurrencyConverter
{
    public static class Program
    {
        public static void Main()
        {
            Console.WriteLine("Usage: Exchange <currency pair> <amount to exchange>");
            var request = Console.ReadLine();
            SetupConfiguration();

            var service = new Service();
            Task.Run(() => service.Start(request));

            Console.ReadLine();
        }

        private static void SetupConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            AppConfiguration.Configuration = builder.Build();
        }
    }
}
