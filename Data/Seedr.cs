using Computer_Store.Models;

namespace Computer_Store.Data
{
    public class Seedr
    {

        public static void Seed(ApplicationDbContext context)
        {
            SeedLaptop(context);
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
            string[] weight = {"2 Kg", "2.1 Kg", "2.2 Kg", "2.3 Kg", "2.4 Kg", "2.5 Kg", "2.6 Kg" };
            string[] wifi = { "11ax, 2×2 + BT5.1", "Intel Wi-Fi 6 AX201(2*2 ax) + BT5", "Killer Wi-Fi 6 AX1650i (2*2 ax) + BT5" };

            Array brands = Enum.GetValues(typeof(Brand));
            Array categories = Enum.GetValues(typeof(ComputerCategory));

            string[] name = { "Legion 5", "Legion 7", "Alienware", "Victus", "Nitro 5", "Helios 300", "ROG", "Zephyrus" };

            Computer computer;
            ComputerSpec spec;
            Random rand = new Random();

            for (int i = 0; i < 20; i++)
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
                    StorageDriveDetail = driveDetail[1],
                    Screen = screenDetail[0],
                    ScreenDetial = screenDetail[0] + " " + screenDetail[1],
                    Weight = weight[rand.Next(0,weight.Length)],
                    WIFI = wifi[rand.Next(0,wifi.Length)]
                };

                Brand brand = (Brand)brands.GetValue(rand.Next(0, brands.Length - 2));
                ComputerCategory category = (ComputerCategory)brands.GetValue(rand.Next(0, categories.Length));

                computer = new Computer
                {
                    Type = type,
                    Brand = brand,
                    Category = category,
                    Price = rand.Next(10_000_000, 100_000_000),
                    Discount = rand.Next(0, 100),
                    Spec = spec,
                    SpecID = spec.Id,
                    Name = String.Join(" ", type, category, brand, name[rand.Next(0,name.Length)], spec.CPU, spec.RAM, spec.GPU, spec.StorageDrive, spec.Screen),
                    Image = @"images\computers\Lenovo\IdeaPad_Gaming_3_white-1_compressed.jpg"
                };
                context.Add(spec);
                context.Add(computer);
                context.SaveChanges();
            }
        }
    }
}
