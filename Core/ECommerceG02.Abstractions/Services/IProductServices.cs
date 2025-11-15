using ECommerceG02.Shared.Common;
using ECommerceG02.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Abstractions.Services
{
    public interface IProductServices
    {
        Task<PaginationResult<ProductDto>> GetAllProductAsync(ProductQueryParms? _ProductQueryParm);

        Task<ProductDto> GetProductByIdAsync(int id);

        Task<IEnumerable<BrandDto>> GetAllBrandsAsync();

        Task<IEnumerable<TypeDto>> GetAllTypesAsync();
    }
}
