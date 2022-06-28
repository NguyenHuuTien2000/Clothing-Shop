using Computer_Store.Models;

namespace Computer_Store.Data
{
    public class Seedr
    {

        public static void Seed(ApplicationDbContext context)
        {
            SeedLaptop(context);
            SeedPart(context);
        }

        public static void SeedLaptop(ApplicationDbContext context)
        {
            Enum.TryParse("Laptop", out ComputerType type);
            if (context.Computers.Where(c => c.Type == type).Count() >= 20)
            {
                return;
            }
            string path = Path.Combine(Environment.CurrentDirectory, "Data", "stuff");

            string[] cpus = File.ReadAllLines(Path.Combine(path, "cpus.txt"));
            string[] os = { "Windows 10 Home", "Windows 11 SL" };
            string[] gpu = File.ReadAllLines(Path.Combine(path, "gpus.txt"));
            string[] ram = File.ReadAllLines(Path.Combine(path, "ram.txt"));
            string[] screen = File.ReadAllLines(Path.Combine(path, "screen.txt"));
            string[] drive = File.ReadAllLines(Path.Combine(path, "drive.txt"));
            string[] weight = { "2 Kg", "2.1 Kg", "2.2 Kg", "2.3 Kg", "2.4 Kg", "2.5 Kg", "2.6 Kg" };
            string[] wifi = { "11ax, 2×2 + BT5.1", "Intel Wi-Fi 6 AX201(2*2 ax) + BT5", "Killer Wi-Fi 6 AX1650i (2*2 ax) + BT5" };

            Array brands = Enum.GetValues(typeof(Brand));
            Array categories = Enum.GetValues(typeof(ComputerCategory));

            Dictionary<string, string[]> nameMap = new Dictionary<string, string[]>();
            nameMap.Add("Acer", new string[] { "Nitro 5", "Aspire 7", "Helios 300", "Helios 500", "Trition 500" });

            nameMap.Add("Asus", new string[] { "Vivobook", "TUF F15", "TUF A17", "ROG", "Strix SCAR", "ROG Zephyrus" });

            nameMap.Add("Dell", new string[] { "G15", "Alienware", "G3" });

            nameMap.Add("Gigabyte", new string[] { "G5", "AERO 5", "Aorus 15P", "U4D", "Aorus 5" });

            nameMap.Add("Lenovo", new string[] { "Ideapad Gaming 3", "Ideapad Gaming 5", "Legion 5", "Legion 7", "Ideapad 5 Pro" });

            nameMap.Add("Msi", new string[] { "Alpha", "Delta", "GP76", "GF65", "Bravo" });

            Computer computer;
            ComputerSpec spec;
            Random rand = new Random();

            for (int i = 0; i < 40; i++)
            {
                string[] cpuDetail = cpus[rand.Next(0, cpus.Length)].Split('-');
                string[] gpuDetail = gpu[rand.Next(0, gpu.Length)].Split('-');
                string[] ramDetail = ram[rand.Next(0, ram.Length)].Split('-');
                string[] screenDetail = screen[rand.Next(0, screen.Length)].Split('-');
                string[] driveDetail = drive[rand.Next(0, drive.Length)].Split('-');
                spec = new ComputerSpec
                {
                    CPU = cpuDetail[0],
                    CPUDetail = cpuDetail[1] + " " + cpuDetail[0],
                    OS = os[rand.Next(0, 1)],
                    RAM = ramDetail[0],
                    RAMDetail = ramDetail[1],
                    GPU = gpuDetail[0],
                    GPUDetail = gpuDetail[1],
                    StorageDrive = driveDetail[0],
                    StorageDriveDetail = driveDetail[0] + " " + driveDetail[1],
                    Screen = screenDetail[0],
                    ScreenDetial = screenDetail[0] + " " + screenDetail[1],
                    Weight = weight[rand.Next(0, weight.Length)],
                    WIFI = wifi[rand.Next(0, wifi.Length)]
                };

                Brand brand = (Brand)brands.GetValue(rand.Next(0, brands.Length - 2));
                ComputerCategory category = (ComputerCategory)categories.GetValue(rand.Next(0, categories.Length));
                string brandName = Enum.GetName(brand);
                string[] nameArray = nameMap[brandName];
                string name = nameArray[rand.Next(0, nameArray.Length)];
                string imgPath = Path.Combine("images", "computers", brandName, rand.Next(1, 6) + ".jpg");

                computer = new Computer
                {
                    Type = type,
                    Brand = brand,
                    Category = category,
                    Price = rand.Next(10_000, 100_001) * 1_000,
                    Discount = rand.Next(0, 20),
                    Spec = spec,
                    SpecID = spec.Id,
                    Name = String.Join(" ", type, category, brand, name, spec.CPU, spec.RAM, spec.GPU, spec.StorageDrive, spec.Screen),
                    Image = imgPath
                };

                context.Add(spec);
                context.Add(computer);
                context.SaveChanges();
            }

        }

        public static void SeedPart(ApplicationDbContext context)
        {
            Enum.TryParse("Part", out PartCategory type);
            if (context.Parts.Where(c => c.Category == type).Count() >= 20)
            {
                return;
            }
            string path = Path.Combine(Environment.CurrentDirectory, "Data", "stuff");


            Array brands = Enum.GetValues(typeof(Brand));
            Array categories = Enum.GetValues(typeof(PartCategory));

            Part part;
            Random rand = new Random();

            for (int i = 0; i < 60; i++)
            {
                PartCategory category = (PartCategory)categories.GetValue(rand.Next(0, categories.Length - 4));
                Brand brand = (Brand)brands.GetValue(rand.Next(0, brands.Length));
                string categoriesName = Enum.GetName(category);
                string imgPath = Path.Combine("images", "parts", categoriesName, rand.Next(1, 7) + ".jpg");

                part = new Part
                {
                    Brand = brand,
                    Category = category,
                    Price = rand.Next(5_000, 20_001) * 1_000,
                    Discount = rand.Next(0, 11),
                    Name = string.Join(" ", category, brand, "Place holder"),
                    Image = imgPath
                };

                context.Add(part);
                context.SaveChanges();
            }
        }

    }
}
