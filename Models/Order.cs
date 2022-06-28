using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Computer_Store.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public string UserID { get; set; }

        public string Status { get; set; }

        [DisplayName("Order Date")]
        public DateTime CreatedDate { get; set; }

        public double? Total { get; set; }

        public string? DisplayTotal
        {
            get { return string.Format("{0:n0}", Total); }
        }

        [DisplayName("Number of Items")]
        public int ItemNum { get; set; } = 0;

        public string PaymentMethod { get; set; }

        public string? DeliveryAddress { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }

    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Product")]
        public int ProductID { get; set; }

        public int Quantity { get; set; }

        public double? Price { get; set; }

        public string? DisplayPrice
        {
            get { return string.Format("{0:n0}", Price); }
        }

        public Product Product { get; set; }

        [Required]
        [ForeignKey("Order")]
        public int OrderID { get; set; }

        public Order Order { get; set; }

    }
}
