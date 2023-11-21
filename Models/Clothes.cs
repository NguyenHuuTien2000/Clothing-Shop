using Computer_Store.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Clothing_Shop.Models
{
    public class Clothes : Product
    {
        [DisplayName("Brand")]
        public ClothesBrand ClothesBrand { get; set; }

        [DisplayName("Size")]
        public ClothesSize ClothesSize { get; set; }
    }

    public enum ClothesBrand
    {
        None,
        [Display(Name = "Louis Vuitton")]
        Louis_Vuitton,
        Gucci,
        Versace,
        Prada,
        Armani,
        Adiddas
    }

    public enum ClothesSize
    {
        Small,
        Medium,
        Large,
        XL,
        XXL
    }
}
