using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Domian.Exceptions.NotFound
{
    public sealed class AddressNotFoundException : NotFoundException
    {
        public AddressNotFoundException(string id)
            : base($"Address with id {id} not found.") { }
    }
}
