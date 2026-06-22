using E_CommerceSystem_API.DTOs;
using E_CommerceSystem_API.Services;
using ECommerecAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceSystem_API.Controllers
{
    [ApiController]
    [Route("api/Product")]
    public class ProductController : ControllerBase
    {
        private readonly ProductServices _productServices;

        public ProductController(ProductServices productServices)
        {
            _productServices = productServices;
        }
        

        [HttpPost("AddProduct")]
        [Authorize(Roles = "Admin")]
        public IActionResult AddProduct(AddProductDTO dto)
        {
            var result = _productServices.AddProduct(dto);

            return Ok(result);
        }

        [HttpPut("UpdateProduct")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateProduct(UpdateProductDTO dto)
        {
            var result = _productServices.UpdateProduct(dto);

            if (result == null)
                return NotFound("Product not found");

            return Ok(result);
        }


        [HttpGet("GetListOfProducts")]
        [Authorize(Roles = "Admin")]
        public IActionResult ListProducts()
        {
            return Ok(_productServices.GetAllProducts());
        }


        [HttpGet("GetProductById")]
        [Authorize]
        public IActionResult GetProductById(int id)
        {
            var product = _productServices.GetProductById(id);

            if (product == null)
                return NotFound("Product not found");

            return Ok(product);
        }
    }
}