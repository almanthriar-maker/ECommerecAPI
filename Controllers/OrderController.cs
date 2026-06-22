using E_CommerceSystem_API.Models;
using E_CommerceSystem_API.Services;
using ECommerecAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;


namespace E_CommerceSystem_API.Controllers
{

        [ApiController]
        [Route("api/Order")]
        [Authorize]
        public class OrderController : ControllerBase
        {
            private readonly OrderServices _orderServices;

            public OrderController(OrderServices orderServices)
            {
                _orderServices = orderServices;
            }

            [HttpGet("GetAllOrdersForUser")]
            public IActionResult ListOrderForUser(int userId)
            {
                var result = _orderServices.GetAllOrdersForUser(userId);

                if (result == null)
                    return NotFound("No orders found for this user.");

                return Ok(result);
            }

            [HttpGet("GetOrderDetailsById")]
            public IActionResult GetOrderById(int id)
            {
                var result = _orderServices.GetOrderById(id);

                if (result == null)
                    return NotFound("Order not found.");

                return Ok(result);
            }
        }
    }
