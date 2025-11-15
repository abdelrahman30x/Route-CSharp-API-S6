using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Shared.ErrorModels
{
    public class ValidationErrorToReturn
    {
        public int StatusCode { get; set; } = (int)HttpStatusCode.BadRequest;
        public string Message { get; set; } = "Validation Errors Occurred";
        public IEnumerable<ValidationError> Errors { get; set; } = Enumerable.Empty<ValidationError>();
    }
}
