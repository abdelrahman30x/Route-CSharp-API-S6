using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ECommerceG02.Domian.Exceptions.NotFound
{
    public class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException(string message = "User not found")
            : base(message) { }
    }
}