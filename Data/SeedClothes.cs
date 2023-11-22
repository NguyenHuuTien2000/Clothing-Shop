using Clothing_Shop.Models;
using Computer_Store.Data;

namespace Clothing_Shop.Data
{
    public class SeedClothes
    {
        public static void Seed(ApplicationDbContext context)
        {
            var clear = context.Carts.First(c => c.Id == -1);
            context.Carts.Remove(clear);
            context.SaveChanges();
            if (context.Clothes.Count() >= 20)
            {
                return;
            }

            Array brands = Enum.GetValues(typeof(ClothesBrand));
            Array sizes = Enum.GetValues(typeof(ClothesSize));
            string[] names = { "Emerald Apparel", "Threads & Treads", "The Style Project", "Couture Engine", "Glam Shack", "True Armoire", "The Sleek Studio" };

            Random rand = new Random();

            for (int i = 0; i < 40; i++)
            {
                
                string productCode = rand.Next(0,2) == 0 ? "f" : "n";
                string imgPath = Path.Combine("img", "products", productCode + rand.Next(1, 9) + ".jpg");

                ClothesBrand brand = (ClothesBrand)brands.GetValue(rand.Next(0, brands.Length));
                ClothesSize size = (ClothesSize)sizes.GetValue(rand.Next(0, sizes.Length));

                Clothes clothes = new Clothes()
                {
                    Name = names[rand.Next(0,names.Length)],
                    Price = rand.Next(20, 100),
                    Image = imgPath,
                    //ClothesSize = size,
                    ClothesBrand = brand
                };
                context.Add(clothes);
            }
            context.SaveChanges();
        }
    }
}
