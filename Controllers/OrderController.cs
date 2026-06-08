using E_CommerceSystem_API.Models;
using E_CommerceSystem_API.Services;
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
        public ApplicationDbContext _context;
        private readonly LoggingService _log;
        public OrderController(ApplicationDbContext context, LoggingService log)
        {
            _context = context;
            LoggingService _log;
        }



        [HttpGet("GetAllOrdersForUser")]
        public IActionResult ListOrderForUser(int userId)
        {

            //_log.Log($"GetAllOrdersForUser called. UserId={userId}");

            var orders = _context.Orders
                .Where(o => o.UserId == userId)
                .ToList();

            var result = new List<object>();

            foreach (var order in orders)
            {
                var products = _context.OrderProducts
                    .Where(op => op.OrderId == order.OrderId)
                    .ToList();

                foreach (var product in products)
                {
                    var productInfo = _context.Products.FirstOrDefault(p => p.ProductId == product.ProductId);

                    var userInfo = _context.Users.FirstOrDefault(u => u.UserId == order.UserId);

                    result.Add(new
                    {
                        order.UserId,
                        UserName = userInfo?.Name,
                        order.OrderId,
                        product.ProductId,
                        ProductName = productInfo?.Name,
                        product.Quantity
                    });
                }
            }

            if (!result.Any())
            {
                _log.Log($"No orders found for UserId={userId}");
                return NotFound("No orders found for this user.");
            }

            // _log.Log($"Returned {result.Count} order items for UserId={userId}");
            return Ok(result);
        }




        [HttpGet("GetOrderDetailsById")]
        public IActionResult GetOrderById(int id)
        {

           // _log.Log($"GetOrderDetailsById called. OrderId={id}");


            var order = _context.Orders
                .FirstOrDefault(o => o.OrderId == id);

            if (order == null)
            {
                _log.Log($"Order not found. OrderId={id}");
                return NotFound("Order not found.");
            }

            var user = _context.Users
                .FirstOrDefault(u => u.UserId == order.UserId);

            var result = _context.OrderProducts
                .Where(op => op.OrderId == id)
                .Select(op => new
                {
                    order.OrderId,
                    order.OrderDate,
                    order.UserId,
                    UserName = user.Name,

                    op.ProductId,
                    ProductName = op.Product.Name,
                    ProductPrice = op.Product.Price,
                    op.Quantity
                })
                .ToList();


           // _log.Log($"Order details returned. OrderId={id}, UserId={order.UserId}");
            return Ok(result);
        }




    }

    }
