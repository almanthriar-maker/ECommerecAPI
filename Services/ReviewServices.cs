using E_CommerceSystem_API;
using E_CommerceSystem_API.DTOs;
using E_CommerceSystem_API.Models;
using E_CommerceSystem_API.Services;

namespace ECommerecAPI.Services
{
    public class Review_Services
    {
            private readonly ApplicationDbContext _context;
            private readonly LoggingService _log;

            public Review_Services(ApplicationDbContext context,LoggingService log)
            {
                _context = context;
                _log = log;
            }

            public object AddReview(AddReviewDTO reviewDto)
            {
                _log.Log($"AddReview called. UserId={reviewDto.UserId}, ProductId={reviewDto.ProductId}");

                var product = _context.Products.FirstOrDefault(p => p.ProductId == reviewDto.ProductId);

                if (product == null)
                {
                    _log.Log($"Review failed. Product {reviewDto.ProductId} not found");
                    return "ProductNotFound";
                }

                var user = _context.Users.FirstOrDefault(u => u.UserId == reviewDto.UserId);

                if (user == null)
                {
                    _log.Log($"Review failed. User {reviewDto.UserId} not found");
                    return "UserNotFound";
                }

                bool purchased = _context.OrderProducts.Any(op =>op.ProductId == reviewDto.ProductId &&op.order.UserId == reviewDto.UserId);

                if (!purchased)
                {
                    _log.Log($"Review rejected. User {reviewDto.UserId} never purchased Product {reviewDto.ProductId}");
                    return "NotPurchased";
                }

                bool alreadyReviewed = _context.Reviews.Any(r =>r.UserId == reviewDto.UserId &&r.ProductId == reviewDto.ProductId);

                if (alreadyReviewed)
                {
                    _log.Log($"Review rejected. User {reviewDto.UserId} already reviewed Product {reviewDto.ProductId}");
                    return "AlreadyReviewed";
                }

                Review review = new()
                {
                    ProductId = reviewDto.ProductId,
                    UserId = reviewDto.UserId,
                    Rating = reviewDto.Rating,
                    Comment = reviewDto.Comment,
                    ReviewDate = DateTime.Now
                };

                _context.Reviews.Add(review);
                _context.SaveChanges();

                var averageRating = _context.Reviews.Where(r => r.ProductId == reviewDto.ProductId).Average(r => r.Rating);

                return new
                {
                    Message = "Review added successfully",
                    ReviewId = review.ReviewId,
                    OverallRating = averageRating
                };
            }

            public object GetAllReviews()
            {
                _log.Log("GetAllReview called");

                var reviews = _context.Reviews.Select(r => new
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

                return reviews;
            }

            public object UpdateReview(UpdateReviewDTO reviewDto)
            {
                _log.Log($"UpdateReview called. ReviewId={reviewDto.ReviewId}");

                var review = _context.Reviews.FirstOrDefault(r => r.ReviewId == reviewDto.ReviewId);

                if (review == null)
                {
                    _log.Log($"Update failed. Review {reviewDto.ReviewId} not found");
                    return null;
                }

                review.Rating = reviewDto.Rating;
                review.Comment = reviewDto.Comment;
                review.ReviewDate = reviewDto.ReviewDate;

                _context.SaveChanges();

                _log.Log($"Review updated successfully. ReviewId={review.ReviewId}");

                return new
                {
                    Message = "Review updated successfully",
                    ReviewId = review.ReviewId
                };
            }

            public bool RemoveReview(int id)
            {
                _log.Log($"RemoveReview called. ReviewId={id}");

                var review = _context.Reviews.Find(id);

                if (review == null)
                {
                    _log.Log($"Delete failed. Review {id} not found");
                    return false;
                }

                _context.Reviews.Remove(review);
                _context.SaveChanges();

                _log.Log($"Review deleted successfully. ReviewId={id}");

                return true;
            }
        }
    }
