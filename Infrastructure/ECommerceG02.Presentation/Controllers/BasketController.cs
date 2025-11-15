using ECommerceG02.Abstractions.Services;
using ECommerceG02.Shared.DTOs.BasketDto_s;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceG02.Presentation.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class BasketController(IManagerServices _ManagerService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<BasketDto>> GetBasketById(string key)
        {
            var basket = await _ManagerService.BasketServices.GetBasketAsync(key);
            return Ok(basket);
        }
        [HttpPost]
        public async Task<ActionResult<BasketDto>> CreateUpdateBasket(BasketDto basket)
        {
            var createdOrUpdatedBasket = await _ManagerService.BasketServices.CreateUpdateBasketAsync(basket);
            return Ok(createdOrUpdatedBasket);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBasket(string id)
        {
            var result =  await _ManagerService.BasketServices.DeleteBasketAsync(id);

            return Ok(result);
        }

    }
}
