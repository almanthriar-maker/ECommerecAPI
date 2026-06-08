namespace E_CommerceSystem_API.DTOs
{
    public class UpdateReviewDTO
    {
        public int ReviewId { get; set; }

        public int Rating { get; set; }

        public string? Comment { get; set; }

        public DateTime? ReviewDate { get; set; }
    }
}
