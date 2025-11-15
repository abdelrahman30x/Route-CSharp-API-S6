using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Domian.Exceptions.Unauthorized
{
    public class UnauthorizedException : AppException
    {
        public UnauthorizedException(string message = "Unauthorized")
            : base(message, 401, "UNAUTHORIZED") { }
    }
}
