using E_CommerceSystem_API;
using E_CommerceSystem_API.DTOs;
using E_CommerceSystem_API.Models;
using E_CommerceSystem_API.Services;

namespace ECommerecAPI.Services
{
        public class ProductServices
        {
            private readonly ApplicationDbContext _context;
            private readonly LoggingService _log;

            public ProductServices(ApplicationDbContext context,LoggingService log)
            {
                _context = context;
                _log = log;
            }

            public object AddProduct(AddProductDTO productDto)
            {
                _log.Log($"AddProduct called. Product Name={productDto.Name}");

                Product p = new Product
                {
                    Name = productDto.Name,
                    Description = productDto.Description,
                    Price = productDto.Price,
                    Stock = productDto.Stock
                };

                _context.Products.Add(p);
                _context.SaveChanges();

                _log.Log($"Product added successfully. ProductId={p.ProductId}");

                return new
                {
                    Message = "Product added successfully",
                    ProductId = p.ProductId
                };
            }

            public object UpdateProduct(UpdateProductDTO updateDto)
            {
                _log.Log($"UpdateProduct called. ProductId={updateDto.ProductId}");

                var product = _context.Products
                    .FirstOrDefault(x => x.ProductId == updateDto.ProductId);

                if (product == null)
                {
                    _log.Log($"Update failed. ProductId={updateDto.ProductId} not found");
                    return null;
                }

                product.Description = updateDto.Description;
                product.Price = updateDto.Price;
                product.Stock = updateDto.Stock;

                _context.SaveChanges();

                _log.Log($"Product updated successfully. ProductId={product.ProductId}");

                return new
                {
                    Message = "Product updated successfully",
                    ProductId = product.ProductId
                };
            }

            public List<Product> GetAllProducts()
            {
                _log.Log("GetListOfProducts called");

                var products = _context.Products.ToList();

                _log.Log($"Returned {products.Count} products");

                return products;
            }

            public Product GetProductById(int id)
            {
                _log.Log($"GetProductById called. ProductId={id}");

                var product = _context.Products.Find(id);

                if (product == null)
                {
                    _log.Log($"Product not found. ProductId={id}");
                    return null;
                }

                _log.Log($"Product returned successfully. ProductId={id}");

                return product;
            }
        }
    }

