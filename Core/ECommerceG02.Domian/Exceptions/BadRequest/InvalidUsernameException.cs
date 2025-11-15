using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Domian.Exceptions.BadRequest
{
    public class InvalidUsernameException : BadRequestException
    {
        public InvalidUsernameException(string message = "Invalid username provided")
            : base(message, "INVALID_USERNAME") { }
    }
}
