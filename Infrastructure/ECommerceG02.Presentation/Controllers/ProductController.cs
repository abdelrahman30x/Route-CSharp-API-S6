using ECommerceG02.Abstractions.Services;
using ECommerceG02.Shared.Common;
using ECommerceG02.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Presentation.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ProductController(IManagerServices _ManagerService) : ControllerBase

    {
        [HttpGet]
        public async Task<ActionResult<PaginationResult<ProductDto>>> GetAllProducts([FromQuery] ProductQueryParms? _ProductQueryParm)
        {
            var products = await _ManagerService.ProductServices.GetAllProductAsync(_ProductQueryParm);
            return Ok(products);
        }
         
        [HttpGet("Brands")]
        public async Task<ActionResult<IEnumerable<BrandDto>>> GetAllBrands()
        {
            var brands = await _ManagerService.ProductServices.GetAllBrandsAsync();
            return Ok(brands);
        }

        [HttpGet("Types")]
        public async Task<ActionResult<IEnumerable<TypeDto>>> GetAllTypes()
        {
            var Types = await _ManagerService.ProductServices.GetAllTypesAsync();
            return Ok(Types);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductByID(int id)
        {
            var Product = await _ManagerService.ProductServices.GetProductByIdAsync(id);
            return Ok(Product);
        }
    }
}
