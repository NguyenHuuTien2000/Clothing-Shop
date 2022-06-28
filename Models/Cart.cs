using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Computer_Store.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
       
        public ICollection<CartItem>? CartItems { get; set; }

        public double? SumPayment { get; set; } = 0;

        public string? DisplaySumPayment
        {
            get { return string.Format("{0:n0}", SumPayment); }
        }
    }

    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Product")]
        public int ProductID { get; set; }

        public int Quantity { get; set; } = 0;

        public double CustomDiscount { get; set; } = 0;

        public double? Price
        {
            get { return (Quantity * Product.FinalPrice) * (1 - CustomDiscount/100); }
        }

        public string? DisplayPrice { 
            get { return string.Format("{0:n0}", Price); } 
        }

        public Product Product { get; set; }

        [Required]
        [ForeignKey("Cart")]
        public int CartID { get; set; }

        public Cart MyCart { get; set; }
    }
}
