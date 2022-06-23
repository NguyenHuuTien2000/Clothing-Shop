using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Computer_Store.Models
{
    public class HistoryItems
    {
        public int Id { get; set; }

        public Product Product{ get; set; }

        [Required]
        [ForeignKey("History")]
        public int HistoryID { get; set; }

        public History MyHistory  { get; set; }


    }
}
