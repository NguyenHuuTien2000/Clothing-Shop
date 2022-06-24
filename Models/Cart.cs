using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Computer_Store.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }

        public ICollection<CartItems>? CartItems { get; set; }
    }

    public class CartItems
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Product")]
        public int ProductID { get; set; }

        public Product Product { get; set; }

        [Required]
        [ForeignKey("Cart")]
        public int CartID { get; set; }

        public Cart MyCart { get; set; }
    }
}
