namespace Computer_Store.Models
{
    public class Part : Product
    {
        public PartCategory Category { get; set; }

        public string? SummarySpec { get; set; }
    }

    public enum PartCategory
    {
        Accessory,
        Component,
    }
}
