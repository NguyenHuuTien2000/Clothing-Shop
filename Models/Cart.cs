namespace Computer_Store.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public IEnumerable<CartItems> CItems { get; set; }
    }
}
