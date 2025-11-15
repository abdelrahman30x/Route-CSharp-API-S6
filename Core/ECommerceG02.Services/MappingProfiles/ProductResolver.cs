using AutoMapper;
using ECommerceG02.Domian.Models.Products;
using ECommerceG02.Shared.DTOs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Services.MappingProfiles
{
    public class ProductResolver(IConfiguration configuration) : IValueResolver<Product, ProductDto, string>
    {
        public string Resolve(Product source, ProductDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return $"{configuration.GetSection("Urls")["BaseUrl"]}{source.PictureUrl}";
            }
            return null;
        }
    }
}
