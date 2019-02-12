using CurrencyConverter.Domain.Services.Interfaces;
using System;

namespace CurrencyConverter.Domain.Services
{
    public class OutputService : IOutputService
    {
        public void Print(string rate)
        {
            Console.WriteLine(rate);
        }
    }
}
