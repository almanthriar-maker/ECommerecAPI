using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace E_CommerceSystem_API.Models
{
    [PrimaryKey(nameof(OrderId), nameof(ProductId))]
    public class OrderProducts
    {
      
        [Required]
        [Range(1, int.MaxValue,ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }


        [ForeignKey("product")]

        public int ProductId { get; set; }

        //navigation properties
        public virtual Product Product { get; set; }
        

        [ForeignKey("Order")]
        public int OrderId { get; set; }

        //navigation properties
        public virtual Order order { get; set; }
        

    }



}

