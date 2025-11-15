using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Domian.Exceptions.Forbidden
{
    public class ForbiddenException : AppException
    {
        public ForbiddenException(string message = "Forbidden")
            : base(message, 403, "FORBIDDEN") { }
    }
}
