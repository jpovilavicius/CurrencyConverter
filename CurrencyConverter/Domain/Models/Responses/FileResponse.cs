using System.Collections.Generic;

namespace CurrencyConverter.Domain.Models.Responses
{
    public class FileResponse : Response
    {
        public List<string> Lines { get; set; }

    }
}
