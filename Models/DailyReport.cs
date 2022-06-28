using System.ComponentModel.DataAnnotations;

namespace Computer_Store.Models
{
  
    public class DailyReport
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? DateString { get; set; }
        public double? TotalRevenue { get; set; }
        public double? TotalUnit { get; set; }
        public string? MostBoughtCategory { get; set; }
        public string? SecondBoughtCategory { get; set; }
    }
}
