namespace Computer_Store.Models
{
    public class History
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime CreateDate { get; set; }
        public IEnumerable<HistoryItems> HItems { get; set; }    
    }
}
