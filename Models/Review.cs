using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace E_CommerceSystem_API.Models
{
    public class Review
    {
        [key]
       
        public int ReviewId { get; set; }

        [Required]
        [Range(1, 5,ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        public string? Comment { get; set; }

        public DateTime? ReviewDate { get; set; }


        //relations

        [ForeignKey("user")]
        
        public int UserId { get; set; }

        //navigation property

        
        public virtual User User { get; set; }



        [ForeignKey("product")]
        
        public int ProductId { get; set; }

        //navigation property

        public virtual Product product { get; set; }

     

    }

}

