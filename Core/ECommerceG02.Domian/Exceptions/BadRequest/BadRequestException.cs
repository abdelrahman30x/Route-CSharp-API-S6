using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Domian.Exceptions.BadRequest
{

    public class BadRequestException : AppException
    {
        public BadRequestException(string message, string errorCode = "BAD_REQUEST")
            : base(message, 400, errorCode)
        {
        }

    }
}
   