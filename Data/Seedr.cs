namespace Computer_Store.Data
{
    public class Seedr
    {
        private ApplicationDbContext _context;

        public Seedr(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            
        }

        public void SeedLaptop()
        {
            if (_context.Computers.Where(c => c.Type == "Laptop"))
            {

            }
        }
    }
}
