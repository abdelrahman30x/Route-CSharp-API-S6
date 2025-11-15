using AutoMapper;
using ECommerceG02.Domian.Models.Baskets;
using ECommerceG02.Domian.Models.Identity;
using ECommerceG02.Domian.Models.Products;
using ECommerceG02.Presistence.Identity.Models;
using ECommerceG02.Shared.DTOs;
using ECommerceG02.Shared.DTOs.AddressDtos;
using ECommerceG02.Shared.DTOs.BasketDto_s;
using ECommerceG02.Shared.DTOs.IdentityDto_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Services.MappingProfiles
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<Product, ProductDto>()
            .ForMember(dist => dist.BrandName, options => options.MapFrom(src => src.Brand.Name))
            .ForMember(dist => dist.TypeName, options => options.MapFrom(src => src.Type.Name))
            .ForMember(dist => dist.PictureUrl, options => options.MapFrom<ProductResolver>());

            CreateMap<ProductBrand, BrandDto>();
            CreateMap<ProductType, TypeDto>();

            CreateMap<BasketItem, BasketItemDto>().ReverseMap();
            CreateMap<CustomerBasket, BasketDto>().ReverseMap();

            CreateMap<RegisterDto, ApplicationUser>().ReverseMap();
            CreateMap<ApplicationUser, UserDto>().ReverseMap();
            CreateMap<LoginDto, ApplicationUser>().ReverseMap();
            CreateMap<Address, AddressDto>().ReverseMap();



        }
    }
}
