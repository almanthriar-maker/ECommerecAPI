using System.ComponentModel.DataAnnotations;

namespace E_CommerceSystem_API.DTOs
{
    public class AddReviewDTO
    {
        public int ProductId { get; set; }

        public int UserId { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        public string? Comment { get; set; }

        public DateTime? ReviewDate { get; set; }
        


    }
}
