using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ECommerceG02.Domian.Exceptions.Unauthorized
{

    public class InvalidTokenException : UnauthorizedException
    {
        public InvalidTokenException(string message = "Invalid token")
            : base(message) { }
    }
}