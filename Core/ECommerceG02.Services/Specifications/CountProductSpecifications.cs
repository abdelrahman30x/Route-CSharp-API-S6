using ECommerceG02.Domian.Models.Products;
using ECommerceG02.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Services.Specifications
{
    public class CountProductSpecifications: BaseSpecification<Product, int>

    {
        public CountProductSpecifications(ProductQueryParms productQueryParms)
     : base(p => (
             (!productQueryParms.BrandId.HasValue || p.BrandId == productQueryParms.BrandId)
             && (!productQueryParms.TypeId.HasValue || p.TypeId == productQueryParms.TypeId)
             && (string.IsNullOrEmpty(productQueryParms.SearchValue)
                 || p.Name.ToLower().Contains(productQueryParms.SearchValue.ToLower()))
         ))
        {
        }
    }
}
