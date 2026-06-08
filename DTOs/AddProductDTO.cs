using System.ComponentModel.DataAnnotations;

namespace E_CommerceSystem_API.DTOs
{
    public class AddProductDTO
    {
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative")]
        public int Stock { get; set; }


    }
}
