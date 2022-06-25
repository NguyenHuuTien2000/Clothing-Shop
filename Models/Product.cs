using System.ComponentModel.DataAnnotations;

namespace Computer_Store.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"[()%.″\-\w ]*", ErrorMessage = "Apart from alphanumeric, only these special characters are allowed: ()%.″-")]
        public string? Name { get; set; }

        [Required]
        public double? Price { get; set; }

        [Range(0, 100, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public double? Discount { get; set; }

        public string? Image { get; set; }

        public Brand Brand { get; set; }


        public int? Sell { get; set; } = 0;


        public string? DisplayPrice
        {
            get { return string.Format("{0:n0}", Price); }
        }

        public double? FinalPrice
        {
            get { return Discount > 0? Price * (1 - Discount / 100) : Price; }
        } 

        public string? DisplayDiscountedPrice
        {
            get { return string.Format("{0:n0}", FinalPrice); }
        }

        public string? DisplayDiscount
        {
            get { return $"-{Discount}%"; }
        }
    }
}
