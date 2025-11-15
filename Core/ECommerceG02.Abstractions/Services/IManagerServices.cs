using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Abstractions.Services
{
    public interface IManagerServices
    {
        public IProductServices ProductServices { get; }
        public IBasketServices BasketServices { get; }
    }
}

