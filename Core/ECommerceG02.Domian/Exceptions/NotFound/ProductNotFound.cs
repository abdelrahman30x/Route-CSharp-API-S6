using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Domian.Exceptions.NotFound
{
    public sealed class ProductNotFoundException : NotFoundException
    {
        public ProductNotFoundException(int id)
            : base($"Product with id {id} not found.") { }
    }
}