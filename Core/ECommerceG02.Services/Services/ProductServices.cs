using AutoMapper;
using ECommerceG02.Abstractions.Services;
using ECommerceG02.Domian.Contacts.UOW;
using ECommerceG02.Domian.Exceptions;
using ECommerceG02.Domian.Exceptions.NotFound;
using ECommerceG02.Domian.Models.Products;
using ECommerceG02.Services.Specifications;
using ECommerceG02.Shared.Common;
using ECommerceG02.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Services.Services
{
    public class ProductServices(IUnitOfWork _UnitOfWork, IMapper _Mapper) : IProductServices
    {
        public async Task<IEnumerable<BrandDto>> GetAllBrandsAsync()
        {
            var repo = _UnitOfWork.GetReposatory<ProductBrand, int>();
            var Brands = await repo.GetAllAsync();
            var BrandsDto = _Mapper.Map<IEnumerable<ProductBrand>, IEnumerable<BrandDto>>(Brands);
            return BrandsDto;
        }

        public async Task<PaginationResult<ProductDto>> GetAllProductAsync(ProductQueryParms? _ProductQueryParm)
        {
            var spec = new ProductSpecifications(_ProductQueryParm);
            var products = await _UnitOfWork.GetReposatory<Product, int>().GetAllWithSpecificationAsync(spec);
            var CountSpec = new CountProductSpecifications(_ProductQueryParm);

            //var products = await _UnitOfWork.GetReposatory<Product, int>().GetAllAsync();
            var ProductsDto = _Mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);
            var Result = new PaginationResult<ProductDto>
            {
                PageIndex = _ProductQueryParm?.PageIndex ?? 1,
                PageSize = _ProductQueryParm?.PageSize ?? 10,
                TotalCount = await _UnitOfWork.GetReposatory<Product, int>().GetCountWithSpecificationAsync(CountSpec),
                Data = ProductsDto
            };
            return Result;
        }

        public async Task<IEnumerable<TypeDto>> GetAllTypesAsync()
        {
            var types = await _UnitOfWork.GetReposatory<ProductType, int>().GetAllAsync();
            var typedto = _Mapper.Map<IEnumerable<ProductType>, IEnumerable<TypeDto>>(types);
            return typedto;
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {

            var spec = new ProductSpecifications(id);
            var product = await _UnitOfWork.GetReposatory<Product, int>().GetByIdWithSpecifiactionAsync(spec);
            if (product == null) throw new ProductNotFoundException(id);
            var productdto = _Mapper.Map<Product, ProductDto>(product);

            return productdto;

        }
    }
}
