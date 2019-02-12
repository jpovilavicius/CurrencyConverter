using CurrencyConverter.Domain.Configuration;
using CurrencyConverter.Domain.Models;
using CurrencyConverter.Domain.Models.Responses;
using CurrencyConverter.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CurrencyConverter.Domain.Services
{
    public class FileService : IFileService
    {
        public async Task<FileResponse> ReadAsync()
        {
            try
            {
                var fileName = Path.GetFullPath(@"..\..\..\") + AppConfiguration.FileName;

                using (var reader = new StreamReader(fileName))
                {
                    var file = await reader.ReadToEndAsync();
                    var lines = file.Split("\r\n");
                    var response = new FileResponse
                    {
                        StatusCode = StatusCode.Ok,
                        Lines = new List<string>()
                    };

                    response.Lines.AddRange(lines);
                    return response;
                }
            }
            catch (Exception e)
            {
                return new FileResponse
                {
                    StatusCode = StatusCode.ServerError,
                    Message = $"Error occured on ReadService.ReadAsync: {e.Message}"
                };

            }
        }
    }
}
