using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace E_CommerceSystem_API.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$",ErrorMessage = "Password must be at least 8 characters and contain uppercase, number, and special character.")]
        public string Password { get; set; }

        [Required]
        
        public string Phone { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;


        //relations 

      
        //public virtual ICollection<Order> Orders { get; set; }
        public List<Order> Orders { get; set; } = new();

        

        //public virtual ICollection<Review> Reviews { get; set; }
        public List<Review> Reviews { get; set; } = new();

    }
}
