using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Computer_Store.Models
{
    public class Computer : Product
    {
        [Display(Name = "Category")]
        public ComputerCategory Category { get; set; }

        [Display(Name = "Type")]
        public ComputerType Type { get; set; }

        [ForeignKey("PCSpec")]
        public int SpecID { get; set; }

        public ComputerSpec? Spec { get; set; }
    }

    public enum ComputerType
    {
        Laptop,
        Desktop
    }

    public enum ComputerCategory
    {
        Gaming,
        Workstation,

        [Display(Name = "Content Creation")]
        Content_Creation
    }

    public class ComputerSpec
    {
        public int Id { get; set; }

        public Computer? Computer { get; set; }

        [Required]
        public string? CPU { get; set; }
        public string? CPUDetail { get; set; }

        [Required]
        public string? OS { get; set; }

        [Required]
        public string? RAM { get; set; }
        public string? RAMDetail { get; set; }

        [Required]
        public string? GPU { get; set; }
        public string? GPUDetail { get; set; }

        public string? Motherboard { get; set; }
        public string? MotherboardDetail { get; set; }

        public string? Screen { get; set; }
        public string? ScreenDetial { get; set; }

        [Required]
        public string? StorageDrive { get; set; }
        public string? StorageDriveDetail { get; set; }

        public string? WIFI { get; set; }

        public string? Weight { get; set; }

        public string? PowerSupply { get; set; }
    }
}
