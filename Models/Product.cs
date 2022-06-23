using System.ComponentModel.DataAnnotations;

namespace Computer_Store.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"[()%.″\-\w ]*", ErrorMessage = "Name can only inlcude special character: ()%.″-")]
        public string? Name { get; set; }

        [Required]
        public double? Price { get; set; }

        [Range(0, 100, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public double? Discount { get; set; }

        public string? Image { get; set; }

        public Brand Brand { get; set; }

        public string? DisplayPrice
        {
            get { return String.Format("{0:n0}", Price); }
        }

        public string? DisplayDiscountedPrice
        {
            get { return String.Format("{0:n0}", Price * (1 - Discount/100)); }
        }

        public string? DisplayDiscount
        {
            get { return $"-{Discount}%"; }
        }
    }
}
