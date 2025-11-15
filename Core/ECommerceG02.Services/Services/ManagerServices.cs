using AutoMapper;
using ECommerceG02.Abstractions.Services;
using ECommerceG02.Domian.Contacts.Repos;
using ECommerceG02.Domian.Contacts.UOW;
using ECommerceG02.Domian.Models.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace ECommerceG02.Services.Services
{
    public class ManagerServices(
        IMapper map,
        IUnitOfWork uow,
        IBasketRepository basketRepository,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor,
        ILogger<AuthenticationServices> logger
    ) : IManagerServices
    {
        private readonly Lazy<IProductServices> LazyProduct_services =
            new(() => new ProductServices(uow, map));
        public IProductServices ProductServices => LazyProduct_services.Value;

        private readonly Lazy<IBasketServices> LazyBasket_services =
            new(() => new BasketServices(basketRepository, map));
        public IBasketServices BasketServices => LazyBasket_services.Value;

        private readonly Lazy<IAuthenticationServices> LazyAuth_services =
            new(() => new AuthenticationServices(userManager, signInManager, roleManager, configuration, httpContextAccessor, map, logger));
        public IAuthenticationServices AuthenticationServices => LazyAuth_services.Value;
    }
}
