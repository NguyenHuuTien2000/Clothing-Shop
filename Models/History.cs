using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Computer_Store.Models
{
    public class History
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public String UserId { get; set; }

        public DateTime CreateDate { get; set; }

        public ICollection<HistoryItems> HistoryItems { get; set; }    
    }
    public class HistoryItems
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public Product Product { get; set; }

        [Required]
        [ForeignKey("History")]
        public int HistoryID { get; set; }

        public History MyHistory { get; set; }
    }
}
