using E_CommerceSystem_API.DTOs;
using E_CommerceSystem_API.Models;
using E_CommerceSystem_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;

namespace E_CommerceSystem_API.Controllers
{



    [ApiController]
    [Route("api/Product")]
    public class ProductController : ControllerBase
    {
 
        public ApplicationDbContext _context;
        public LoggingService _log;
        public ProductController(ApplicationDbContext context, LoggingService log)

        {
            _context = context;
            _log = log;

        }


        [HttpPost("AddProduct")]
        [Authorize(Roles = "Admin")]
        public IActionResult AddProduct(AddProductDTO productdto)
        {

            _log.Log($"AddProduct called. Product Name={productdto.Name}");
            Product p =new Product();
            p.Name = productdto.Name;
            p.Description = productdto.Description;
            p.Price = productdto.Price;
            p.Stock = productdto.Stock;


            _context.Products.Add(p);
            _context.SaveChanges();

            _log.Log($"Product added successfully. ProductId={p.ProductId}");

            return Ok(" Product added successfully with ID : " + p.ProductId);
        }


        [HttpPut("UpdateProduct")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateProduct(UpdateProductDTO updatedto)
        {
            _log.Log($"UpdateProduct called. ProductId={updatedto.ProductId}");

            var p = _context.Products.FirstOrDefault(x => x.ProductId == updatedto.ProductId);

            if (p == null)
            {
                _log.Log($"Update failed. ProductId={updatedto.ProductId} not found");
                return NotFound("Product not found");
            }

            p.Description = updatedto.Description;
            p.Price = updatedto.Price;
            p.Stock = updatedto.Stock;

            _context.SaveChanges();
            _log.Log($"Product updated successfully. ProductId={p.ProductId}");

            return Ok("Product updated successfully with ID: " + p.ProductId);
        }


        [HttpGet("GetListOfProducts")]
        [Authorize(Roles = "Admin")]
        public IActionResult ListProducts()
        {
            _log.Log("GetListOfProducts called");
            var Product = _context.Products.ToList();
            _log.Log($"Returned {Product.Count} products");
            return Ok(Product);
        }



        [HttpGet("GetProductById")]
        [Authorize]
        public IActionResult GetProductById(int id)
        {
            _log.Log($"GetProductById called. ProductId={id}");
            var Product = _context.Products.Find(id);
            if (Product== null)
            {
                _log.Log($"Product not found. ProductId={id}");
                return NotFound("Product not found");
            }
            _log.Log($"Product returned successfully. ProductId={id}");
            return Ok( Product);
        }




    }
}
