﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Computer_Store.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public IEnumerable<CartItems> CItems { get; set; }
    }

    public class CartItems
    {

        public int Id { get; set; }

        public Product Product { get; set; }

        [Required]
        [ForeignKey("Cart")]
        public int CartID { get; set; }

        public Cart MyCart { get; set; }
    }
}