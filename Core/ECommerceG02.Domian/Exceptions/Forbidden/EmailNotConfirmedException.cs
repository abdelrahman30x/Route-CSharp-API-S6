using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Domian.Exceptions.Forbidden
{
    public class EmailNotConfirmedException : ForbiddenException
    {
        public EmailNotConfirmedException(string message = "Email not confirmed")
            : base(message) { }
    }
}