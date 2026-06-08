using E_CommerceSystem_API.DTOs;
using E_CommerceSystem_API.Models;
using E_CommerceSystem_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Diagnostics.Metrics;


namespace E_CommerceSystem_API.Controllers
{


    [ApiController]
    [Route("api/Review")]
    [Authorize]
    public class ReviewController : ControllerBase
    {
        public ApplicationDbContext _context;
        public LoggingService _log;
        public ReviewController(ApplicationDbContext context, LoggingService log)

        {
            _context = context;
            _log = log;

        }



        [HttpPost("AddReview")]
      
        public IActionResult AddReview(AddReviewDTO reviewdto)
        {
            _log.Log($"AddReview called. UserId={reviewdto.UserId}, ProductId={reviewdto.ProductId}");

            var product = _context.Products.FirstOrDefault(p => p.ProductId == reviewdto.ProductId);

            if (product == null)
            {
                _log.Log($"Review failed. Product {reviewdto.ProductId} not found");

                return NotFound("Product not found");
            }

            var user = _context.Users.FirstOrDefault(u => u.UserId == reviewdto.UserId);

            if (user == null)
            {
                _log.Log($"Review failed. User {reviewdto.UserId} not found");

                return NotFound("User not found");
            }

            // Check if user purchased this product
            bool purchased = _context.OrderProducts.Any(op =>op.ProductId == reviewdto.ProductId &&op.order.UserId == reviewdto.UserId);

            if (!purchased)
            {
                _log.Log($"Review rejected. User {reviewdto.UserId} never purchased Product {reviewdto.ProductId}");

                return BadRequest("You can only review products you purchased.");
            }

            // Check if user already reviewed this product
            bool alreadyReviewed = _context.Reviews.Any(r =>r.UserId == reviewdto.UserId &&r.ProductId == reviewdto.ProductId);

            if (alreadyReviewed)
            {
                _log.Log($"Review rejected. User {reviewdto.UserId} already reviewed Product {reviewdto.ProductId}");

                return BadRequest("You already reviewed this product.");
            }

            Review r = new Review()
            {
                ProductId = reviewdto.ProductId,
                UserId = reviewdto.UserId,
                Rating = reviewdto.Rating,
                Comment = reviewdto.Comment,
                ReviewDate = DateTime.Now
            };

            _context.Reviews.Add(r);
            _context.SaveChanges();

            // Recalculate overall rating
            var averageRating = _context.Reviews.Where(rv => rv.ProductId == reviewdto.ProductId).Average(rv => rv.Rating);

            return Ok(new
            {
                Message = "Review added successfully",
                ReviewId = r.ReviewId,
                OverallRating = averageRating
            });
        }


        [HttpGet("GetAllReview")]
       
        public IActionResult ListReview()
        {
            _log.Log("GetAllReview called");

            var reviews = _context.Reviews
                .Select(r => new
                {
                    ReviewId = r.ReviewId,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    ReviewDate = r.ReviewDate,
                    UserId = r.UserId,
                    UserName = r.User.Name,      
                    ProductId = r.ProductId,
                    ProductName = r.product.Name
                })
                .ToList();
            _log.Log($"Returned {reviews.Count} reviews");

            return Ok(reviews);
        }






        [HttpPut("UpdateReview")]
        
        public IActionResult UpdateReview(UpdateReviewDTO reviewDTO)
        {
            _log.Log($"UpdateReview called. ReviewId={reviewDTO.ReviewId}");
            var review = _context.Reviews
                .FirstOrDefault(r => r.ReviewId == reviewDTO.ReviewId);

            if (review == null)
            {
                _log.Log($"Update failed. Review {reviewDTO.ReviewId} not found");
                return NotFound("Review not found");
            }

            review.Rating = reviewDTO.Rating;
            review.Comment = reviewDTO.Comment;
            review.ReviewDate = reviewDTO.ReviewDate;

            _context.SaveChanges();
            _log.Log($"Review updated successfully. ReviewId={review.ReviewId}");


            return Ok("Review updated successfully with ID: " + review.ReviewId);
        }

        [HttpDelete("RemoveReview")]
        public IActionResult RemoveReview(int id)
        {
            _log.Log($"RemoveReview called. ReviewId={id}");
            var review = _context.Reviews.Find(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                _context.SaveChanges();
                _log.Log($"Review deleted successfully. ReviewId={id}");
                return Ok("Review removed successfully");

            }
            _log.Log($"Delete failed. Review {id} not found");

            return NotFound("Review not found");

        }



    }
}
