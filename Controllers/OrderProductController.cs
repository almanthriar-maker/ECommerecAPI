using E_CommerceSystem_API.DTOs;
using E_CommerceSystem_API.Models;
using E_CommerceSystem_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceSystem_API.Controllers
{




    [ApiController]
    [Route("api/OrderProduct")]
    [Authorize]
    public class OrderProductController: ControllerBase
    {

        public ApplicationDbContext _context;
        public LoggingService _log;
        public OrderProductController(ApplicationDbContext context, LoggingService log)
        {
            _context = context;
            _log = log;
        }

        [HttpPost("PlaceOrder")]
       
        public IActionResult PlaceOrder(PlaceOrderDTO dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == dto.UserId);

            if (user == null)
            {
                return BadRequest("User not found");
            }

            Order order = new Order
            {
                UserId = dto.UserId,
                OrderDate = DateTime.Now,
                TotalAmount = 0
            };

            _context.Orders.Add(order);
            _context.SaveChanges();
            _log.Log(order.OrderId.ToString());

            List<OrderProducts> orderProducts = new List<OrderProducts>();

            foreach (var item in dto.Items)
            {
                var product = _context.Products.FirstOrDefault(p => p.ProductId == item.Pid);

                if (product == null)
                {
                    _log.Log($"Product {item.Pid} not found");
                    return BadRequest($"Product {item.Pid} not found");
                }

                if (product.Stock < item.qnt)
                {
                    _log.Log($"Insufficient stock for {product.Name}");
                    return BadRequest($"not available stock for product {product.Name}");
                }

                _log.Log($"Product {product.ProductId} added. Quantity={item.qnt}");

                product.Stock -= item.qnt;

                order.TotalAmount += product.Price * item.qnt;

                orderProducts.Add(new OrderProducts
                {
                    OrderId = order.OrderId,
                    ProductId = item.Pid,
                    Quantity = item.qnt
                });
            }

            _context.OrderProducts.AddRange(orderProducts);
            _context.SaveChanges();
            _log.Log("OrderProducts added successfully");

            return Ok("Order placed successfully");
        }


    }
}
