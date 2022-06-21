using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Computer_Store.Models
{
    public class Computer : Product
    {
        [Display(Name = "Category")]
        public PCCategory Category { get; set; }

        [Display(Name = "Type")]
        public PCType PCType { get; set; }

        [Display(Name = "Specification")]
        [ForeignKey("PCSpec")]
        public int SpecID { get; set; }

        public PCSpec? PCSpec { get; set; }
    }

    public enum PCType
    {
        Laptop,
        Desktop
    }

    public enum PCCategory
    {
        Gaming,
        Workstation,
        Content_Creation
    }

    public class PCSpec
    {
        public int Id { get; set; }

        public Computer? Computer { get; set; }

        public string? CPU { get; set; }
        public string? OS { get; set; }
        public string? RAM { get; set; }
        public string? GPU { get; set; }
        public string? Motherboard { get; set; }
        public string? Screen { get; set; }
        public string? SSD { get; set; }
        public string? HDD { get; set; }
        public string? WIFI { get; set; }
    }
}
