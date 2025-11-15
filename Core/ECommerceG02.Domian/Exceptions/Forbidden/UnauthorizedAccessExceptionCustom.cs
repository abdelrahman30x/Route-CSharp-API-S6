using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Domian.Exceptions.Forbidden
{
    public class UnauthorizedAccessExceptionCustom : ForbiddenException
    {
        public UnauthorizedAccessExceptionCustom(string message = "Unauthorized access")
            : base(message) { }
    }
}