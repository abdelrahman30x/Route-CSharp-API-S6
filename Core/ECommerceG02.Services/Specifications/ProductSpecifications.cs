using ECommerceG02.Domian.Models.Products;
using ECommerceG02.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Services.Specifications
{
    public class ProductSpecifications : BaseSpecification<Product, int>
    {
        public ProductSpecifications(ProductQueryParms productQueryParms)
     : base(p => (
             (!productQueryParms.BrandId.HasValue || p.BrandId == productQueryParms.BrandId)
             && (!productQueryParms.TypeId.HasValue || p.TypeId == productQueryParms.TypeId)
             && (string.IsNullOrEmpty(productQueryParms.SearchValue)
                 || p.Name.ToLower().Contains(productQueryParms.SearchValue.ToLower()))
         ))
        {
            AddInclude(p => p.Brand);
            AddInclude(p => p.Type);
            switch (productQueryParms.SortingOption)
            {
                case ProductSortingOptions.NameAsc:
                    AddOrderBy(p => p.Name);
                    break;

                case ProductSortingOptions.NameDesc:
                    AddOrderByDesc(p => p.Name);
                    break;

                case ProductSortingOptions.PriceAsc:
                    AddOrderBy(p => p.Price);
                    break;

                case ProductSortingOptions.PriceDesc:
                    AddOrderByDesc(p => p.Price);
                    break;
                default:
                    break;
            }

            ApplyPagination(productQueryParms.PageSize, productQueryParms.PageIndex);

        }

        public ProductSpecifications(int id) : base(p => p.Id == id)
        {
            AddInclude(p => p.Brand);
            AddInclude(p => p.Type);


        }

    }
}
