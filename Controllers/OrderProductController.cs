using E_CommerceSystem_API.DTOs;
using E_CommerceSystem_API.Services;
using ECommerecAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceSystem_API.Controllers
{
    [ApiController]
    [Route("api/OrderProduct")]
    [Authorize]
    public class OrderProductController : ControllerBase
    {
        private readonly OrderProductServices _orderProductServices;

        public OrderProductController(OrderProductServices orderProductServices)
        {
            _orderProductServices = orderProductServices;
        }



        [HttpPost("PlaceOrder")]
        public IActionResult PlaceOrder(PlaceOrderDTO dto)
        {
            var result = _orderProductServices.PlaceOrder(dto);

            if (result is string message)
            {
                return BadRequest(message);
            }

            return Ok(result);
        }
    }
}