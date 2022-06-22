using System.ComponentModel.DataAnnotations;

namespace Computer_Store.Models
{
    public class Part : Product
    {
        [Display(Name = "Category")]
        public PartCategory Category { get; set; }

        [Display(Name = "Description")]
        public string? SummarySpec { get; set; }
    }

    public enum PartCategory
    {
        Accessory,
        Component,
    }
}
