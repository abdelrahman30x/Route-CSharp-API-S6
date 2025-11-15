using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ECommerceG02.Domian.Exceptions.Unauthorized
{
    public class InvalidCredentialsException : UnauthorizedException
    {
        public InvalidCredentialsException(string message = "Invalid credentials")
            : base(message) { }
    }
}