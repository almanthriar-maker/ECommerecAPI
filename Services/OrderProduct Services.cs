using E_CommerceSystem_API;
using E_CommerceSystem_API.DTOs;
using E_CommerceSystem_API.Models;
using E_CommerceSystem_API.Services;

namespace ECommerecAPI.Services
{
    public class OrderProductServices
    {
        private readonly ApplicationDbContext _context;
        private readonly LoggingService _log;

        public OrderProductServices(ApplicationDbContext context, LoggingService log)
        {
            _context = context;
            _log = log;
        }

        public object PlaceOrder(PlaceOrderDTO dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == dto.UserId);

            if (user == null)
                return "UserNotFound";

            Order order = new Order
            {
                UserId = dto.UserId,
                OrderDate = DateTime.Now,
                TotalAmount = 0
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            _log.Log($"Order Created. OrderId={order.OrderId}");

            List<OrderProducts> orderProducts = new();

            foreach (var item in dto.Items)
            {
                var product = _context.Products.FirstOrDefault(p => p.ProductId == item.Pid);

                if (product == null)
                {
                    _log.Log($"Product {item.Pid} not found");
                    return $"Product {item.Pid} not found";
                }

                if (product.Stock < item.qnt)
                {
                    _log.Log($"Insufficient stock for {product.Name}");
                    return $"Not enough stock for product {product.Name}";
                }

                product.Stock -= item.qnt;

                order.TotalAmount += product.Price * item.qnt;

                orderProducts.Add(new OrderProducts
                {
                    OrderId = order.OrderId,
                    ProductId = item.Pid,
                    Quantity = item.qnt
                });

                _log.Log($"Product {product.ProductId} added. Quantity={item.qnt}");
            }

            _context.OrderProducts.AddRange(orderProducts);
            _context.SaveChanges();

            _log.Log($"Order completed successfully. OrderId={order.OrderId}");

            return new
            {
                Message = "Order placed successfully",
                OrderId = order.OrderId,
                TotalAmount = order.TotalAmount
            };
        }
    }
}
