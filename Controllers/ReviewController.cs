using E_CommerceSystem_API.DTOs;
using E_CommerceSystem_API.Services;
using ECommerecAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceSystem_API.Controllers
{
    [ApiController]
    [Route("api/Review")]
    [Authorize]
    public class ReviewController : ControllerBase
    {
        private readonly ReviewServices _reviewServices;

        public ReviewController(ReviewServices reviewServices)
        {
            _reviewServices = reviewServices;
        }

        [HttpPost("AddReview")]
        public IActionResult AddReview(AddReviewDTO dto)
        {
            var result = _reviewServices.AddReview(dto);

            if (result is string error)
            {
                return error switch
                {
                    "ProductNotFound" => NotFound("Product not found"),
                    "UserNotFound" => NotFound("User not found"),
                    "NotPurchased" => BadRequest("You can only review products you purchased."),
                    "AlreadyReviewed" => BadRequest("You already reviewed this product."),
                    _ => BadRequest(error)
                };
            }

            return Ok(result);
        }

        [HttpGet("GetAllReview")]
        public IActionResult ListReview()
        {
            return Ok(_reviewServices.GetAllReviews());
        }

        [HttpPut("UpdateReview")]
        public IActionResult UpdateReview(UpdateReviewDTO dto)
        {
            var result = _reviewServices.UpdateReview(dto);

            if (result == null)
                return NotFound("Review not found");

            return Ok(result);
        }

        [HttpDelete("RemoveReview")]
        public IActionResult RemoveReview(int id)
        {
            bool deleted = _reviewServices.RemoveReview(id);

            if (!deleted)
                return NotFound("Review not found");

            return Ok("Review removed successfully");
        }
    }
}