using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Domian.Exceptions.NotFound
{
    public sealed class BasketNotFoundException : NotFoundException
    {
        public BasketNotFoundException(string Id)
            : base($"Basket with id {Id} not found.") { }
    }
}
