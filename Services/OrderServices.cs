using E_CommerceSystem_API;
using E_CommerceSystem_API.Services;

namespace ECommerecAPI.Services
{
    public class OrderServices
    {
            private readonly ApplicationDbContext _context;
            private readonly LoggingService _log;

            public OrderServices(ApplicationDbContext context, LoggingService log)
            {
                _context = context;
                _log = log;
            }

            public object GetAllOrdersForUser(int userId)
            {
                var orders = _context.Orders.Where(o => o.UserId == userId).ToList();

                var result = new List<object>();

                foreach (var order in orders)
                {
                    var products = _context.OrderProducts.Where(op => op.OrderId == order.OrderId).ToList();

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
                    return null;
                }

                return result;
            }

            public object GetOrderById(int id)
            {
                var order = _context.Orders.FirstOrDefault(o => o.OrderId == id);

                if (order == null)
                {
                    _log.Log($"Order not found. OrderId={id}");
                    return null;
                }

                var user = _context.Users.FirstOrDefault(u => u.UserId == order.UserId);

                var result = _context.OrderProducts.Where(op => op.OrderId == id).Select(op => new
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

                return result;
            }
        }
    }
