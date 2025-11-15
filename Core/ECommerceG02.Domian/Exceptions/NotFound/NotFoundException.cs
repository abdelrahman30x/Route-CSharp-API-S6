using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Domian.Exceptions.NotFound
{
    public class NotFoundException : AppException
    {
        public NotFoundException(string message = "Resource not found")
            : base(message, 404, "NOT_FOUND") { }
    }
}

