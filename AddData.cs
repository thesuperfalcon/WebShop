using System.Text.RegularExpressions;
using WebShop.Models;

namespace WebShop
{
    internal class AddData
    {
        public static void AddProductInfo()
        {
            using var db = new MyDbContext();

            var categories = new[] { "Men", "Women", "Pants", "T-Shirt", "Hoodie" };
            var sizes = new[] { "S", "M", "L", "XL" };
            var suppliers = new[] { "Cocktailorde", "Dressman", "Gucci" };
            var colours = new[] { "Red", "Blue", "Green", "Black", "Gray", "Navy", "White", "Brown", "Purple", "Yellow"};

            foreach (var categoryName in categories)
            {
                var category = new Category { CategoryName = categoryName };
                db.Categories.Add(category);
            }

            foreach (var sizeName in sizes)
            {
                var size = new Models.Size { SizeName = sizeName };
                db.Sizes.Add(size);
            }

            foreach (var supplierName in suppliers)
            {
                var supplier = new ProductSupplier { SupplierName = supplierName };
                db.ProductSuppliers.Add(supplier);
            }

            foreach (var colourName in colours)
            {
                var colour = new Colour { ColourName = colourName };
                db.Colours.Add(colour);
            }

            db.SaveChanges();
        }

        public static void AddCustomerInfo()
        {
            using var db = new MyDbContext();

            var countries = new[] { "Sweden", "Norway" };

            foreach (var countryName in countries)
            {
                var country = new Country { CountryName = countryName };
                db.Countries.Add(country);

                var cities = new Dictionary<string, Country>
                {
                    { "Nyköping", country },
                    { "Stockholm", country },
                    { "Göteborg", country },
                    { "Uppsala", country },
                    { "Umeå", country },
                    { "Oslo", db.Countries.First(c => c.CountryName == "Norway") }
                };

                foreach (var (cityName, countryEntity) in cities)
                {
                    var city = new City { CityName = cityName, Country = countryEntity };
                    db.Cities.Add(city);
                }
            }

            var addresses = new[]
            {
                ("Kungsgatan 21", "Stockholm"),
                ("Storgatan 42", "Göteborg"),
                ("Bränntorp 1", "Stockholm"),
                ("Skogsvägen 19", "Oslo"),
                ("Drottninggatan 89", "Uppsala"),
                ("Nyköpingsvägen 10", "Nyköping")
            };

            foreach (var (addressName, cityName) in addresses)
            {
                var address = new Adress { AdressName = addressName, City = db.Cities.First(c => c.CityName == cityName) };
                db.Adresses.Add(address);
            }

            var firstNames = new[] { "Jens", "Maria", "Pär", "Johanna", "Kalle" };
            var lastNames = new[] { "Svensson", "Göransson", "Eklund", "Karlsson", "Stridh" };

            foreach (var firstName in firstNames)
            {
                var nameEntity = new FirstName { Name = firstName };
                db.FirstName.Add(nameEntity);
            }

            foreach (var lastName in lastNames)
            {
                var nameEntity = new LastName { Name = lastName };
                db.LastName.Add(nameEntity);
            }

            db.SaveChanges();
        }

        public static void AddNewCustomerWithInput()
        {
            using var db = new MyDbContext();

            string firstName = InputHelpers.GetInput("Enter first name: ");

            string lastName = InputHelpers.GetInput("Enter last name: ");

            string addressName = InputHelpers.GetInput("Enter address name: ");

            int postalCode = InputHelpers.GetIntegerInput("Enter postal code: ");

            string cityName = InputHelpers.GetInput("Enter city name: ");

            Console.Write("Enter country name: ");
            string countryName = InputHelpers.GetInput("");

            Console.Write("Enter phone number (or press Enter to skip): ");
            string phoneNumberInput = Console.ReadLine();
            int? phoneNumber = string.IsNullOrEmpty(phoneNumberInput) ? null : InputHelpers.GetIntegerInput(phoneNumberInput);

            string email = InputHelpers.GetInput("Enter email: ");

            string password = InputHelpers.GetInput("Enter password: ");

            bool isAdmin = InputHelpers.GetYesOrNo("Is admin?");

            var existingCountry = db.Countries.FirstOrDefault(c => c.CountryName == countryName) ?? new Country { CountryName = countryName };
            var existingCity = db.Cities.FirstOrDefault(ct => ct.CityName == cityName && ct.CountryId == existingCountry.Id) ?? new City { CityName = cityName, CountryId = existingCountry.Id };
            var existingAddress = db.Adresses.FirstOrDefault(a => a.AdressName == addressName && a.CityId == existingCity.Id) ?? new Adress { AdressName = addressName, CityId = existingCity.Id, PostalCode = postalCode };
            var existingFirstName = db.FirstName.FirstOrDefault(fn => fn.Name == firstName) ?? new FirstName { Name = firstName };
            var existingLastName = db.LastName.FirstOrDefault(ln => ln.Name == lastName) ?? new LastName { Name = lastName };

            var newCustomer = new Customer
            {
                FirstNameId = existingFirstName.Id,
                LastNameId = existingLastName.Id,
                AdressId = existingAddress.Id,
                PhoneNumber = phoneNumber,
                Email = email,
                Password = password,
                IsAdmin = isAdmin,
            };

            db.Customers.Add(newCustomer);
            db.SaveChanges();
        }
        public static void AddMultipleProducts()
        {
            using (var db = new MyDbContext())
            {
                var product1 = CreateProduct("Unicorn", "Fancy unicorn shirt", 99.99, false, 3, new[] { "Women", "T-Shirt" }, db);
                var product2 = CreateProduct("tröja", "fin tröja", 199.99, false, 3, new[] { "Women", "T-Shirt" }, db);
                var product3 = CreateProduct("Hoodie", "fin tröja med luva", 49.99, false, 3, new[] { "Women", "T-Shirt" }, db);
                var product4 = CreateProduct("klänning", "Mysig klänning att vara fin i", 79.99, false, 3, new[] { "Women", "T-Shirt" }, db);
                var product5 = CreateProduct("Men's Shirt", "Stylish men's shirt", 59.99, false, 3, new[] { "Men", "Shirt" }, db);
                var product6 = CreateProduct("Men's Jeans", "Comfortable men's jeans", 89.99, false, 3, new[] { "Men", "Jeans" }, db);
                var product7 = CreateProduct("Men's Jacket", "Warm men's jacket", 129.99, false, 3, new[] { "Men", "Jacket" }, db);
                var product8 = CreateProduct("Men's Sweater", "Cozy men's sweater", 69.99, false, 3, new[] { "Men", "Sweater" }, db);
                var product9 = CreateProduct("Striped Pullover", "Classic striped pullover for men", 79.99, false, 3, new[] { "Men", "Sweater" }, db);
                var product10 = CreateProduct("V-Neck Sweater", "Elegant v-neck sweater for a sophisticated look", 89.99, false, 3, new[] { "Men", "Sweater" }, db);
                var product11 = CreateProduct("Crew Neck Sweater", "Comfortable crew neck sweater for casual wear", 59.99, false, 3, new[] { "Men", "Sweater" }, db);
                var product12 = CreateProduct("Waterproof Parka", "Stylish and waterproof parka for men", 149.99, true, 3, new[] { "Men", "Jacket" }, db);
                var product13 = CreateProduct("Floral Print Tee", "Casual t-shirt with a vibrant floral print", 29.99, false, 3, new[] { "Women", "T-Shirt" }, db);
                var product14 = CreateProduct("Striped V-Neck Shirt", "Chic v-neck shirt with stylish stripes", 34.99, false, 3, new[] { "Women", "T-Shirt" }, db);
                var product15 = CreateProduct("Casual Denim Dress", "Relaxed fit denim dress for a laid-back style", 69.99, false, 3, new[] { "Women", "Dress" }, db);
                var product16 = CreateProduct("Cozy Knit Cardigan", "Warm and comfortable knit cardigan", 54.99, false, 3, new[] { "Women", "Sweater" }, db);
                var product17 = CreateProduct("Leather Jacket", "Stylish leather jacket for a trendy look", 119.99, false, 3, new[] { "Women", "Jacket" }, db);
                var product18 = CreateProduct("Slim Fit Men's Jeans", "Modern slim fit jeans for men", 79.99, false, 3, new[] { "Men", "Jeans" }, db);
                var product19 = CreateProduct("Graphic Print Men's T-Shirt", "Casual men's t-shirt with a cool graphic print", 44.99, false, 3, new[] { "Men", "T-Shirt" }, db);
                var product20 = CreateProduct("Quilted Bomber Jacket", "Fashionable quilted bomber jacket for men", 89.99, false, 3, new[] { "Men", "Jacket" }, db);

                AddProductVariants(db, product1, ("Red", "S", 12), ("Red", "M", 33), ("Red", "L", 2), ("Red", "XL", 5), ("Blue", "S", 12), ("Blue", "M", 1), ("Blue", "L", 4), ("Blue", "XL", 0));
                AddProductVariants(db, product2, ("Red", "S", 12), ("Red", "M", 33), ("Red", "L", 2), ("Red", "XL", 5), ("Blue", "S", 12), ("Blue", "M", 1), ("Blue", "L", 4), ("Blue", "XL", 0));
                AddProductVariants(db, product3, ("Yellow", "S", 12), ("Yellow", "M", 33), ("Yellow", "L", 2), ("Yellow", "XL", 5), ("Blue", "S", 12), ("Blue", "M", 1), ("Blue", "L", 4), ("Blue", "XL", 0));
                AddProductVariants(db, product4, ("Black", "S", 2), ("Black", "M", 10), ("Black", "L", 3), ("Black", "XL", 7), ("Purple", "S", 12), ("Purple", "M", 10), ("Purple", "L", 8), ("Purple", "XL", 0));
                AddProductVariants(db, product5, ("Red", "S", 12), ("Red", "M", 33), ("Red", "L", 2), ("Red", "XL", 5), ("Blue", "S", 12), ("Blue", "M", 1), ("Blue", "L", 4), ("Blue", "XL", 0));
                AddProductVariants(db, product6, ("Green", "S", 15), ("Green", "M", 28), ("Green", "L", 3), ("Green", "XL", 8), ("Black", "S", 12), ("Black", "M", 5), ("Black", "L", 10), ("Black", "XL", 2));
                AddProductVariants(db, product7, ("Gray", "S", 10), ("Gray", "M", 20), ("Gray", "L", 4), ("Gray", "XL", 6), ("Brown", "S", 8), ("Brown", "M", 15), ("Brown", "L", 5), ("Brown", "XL", 3));
                AddProductVariants(db, product8, ("Navy", "S", 18), ("Navy", "M", 25), ("Navy", "L", 7), ("Navy", "XL", 4), ("White", "S", 20), ("White", "M", 22), ("White", "L", 6), ("White", "XL", 1));
                AddProductVariants(db, product9, ("Gray", "S", 10), ("Gray", "M", 20), ("Gray", "L", 4), ("Gray", "XL", 6), ("Brown", "S", 8), ("Brown", "M", 15), ("Brown", "L", 5), ("Brown", "XL", 3));
                AddProductVariants(db, product10, ("Navy", "S", 18), ("Navy", "M", 25), ("Navy", "L", 7), ("Navy", "XL", 4), ("White", "S", 20), ("White", "M", 22), ("White", "L", 6), ("White", "XL", 1));
                AddProductVariants(db, product11, ("Red", "S", 12), ("Red", "M", 33), ("Red", "L", 2), ("Red", "XL", 5), ("Blue", "S", 12), ("Blue", "M", 1), ("Blue", "L", 4), ("Blue", "XL", 0));
                AddProductVariants(db, product12, ("Green", "S", 15), ("Green", "M", 28), ("Green", "L", 3), ("Green", "XL", 8), ("Black", "S", 12), ("Black", "M", 5), ("Black", "L", 10), ("Black", "XL", 2));
                AddProductVariants(db, product13, ("Purple", "S", 12), ("Purple", "M", 10), ("Purple", "L", 8), ("Purple", "XL", 0), ("Black", "S", 2), ("Black", "M", 10), ("Black", "L", 3), ("Black", "XL", 7));
                AddProductVariants(db, product14, ("Yellow", "S", 12), ("Yellow", "M", 33), ("Yellow", "L", 2), ("Yellow", "XL", 5), ("Blue", "S", 12), ("Blue", "M", 1), ("Blue", "L", 4), ("Blue", "XL", 0));
                AddProductVariants(db, product15, ("Red", "S", 12), ("Red", "M", 33), ("Red", "L", 2), ("Red", "XL", 5), ("Blue", "S", 12), ("Blue", "M", 1), ("Blue", "L", 4), ("Blue", "XL", 0));
                AddProductVariants(db, product16, ("Green", "S", 15), ("Green", "M", 28), ("Green", "L", 3), ("Green", "XL", 8), ("Black", "S", 12), ("Black", "M", 5), ("Black", "L", 10), ("Black", "XL", 2));
                AddProductVariants(db, product17, ("Gray", "S", 10), ("Gray", "M", 20), ("Gray", "L", 4), ("Gray", "XL", 6), ("Brown", "S", 8), ("Brown", "M", 15), ("Brown", "L", 5), ("Brown", "XL", 3));
                AddProductVariants(db, product18, ("Navy", "S", 18), ("Navy", "M", 25), ("Navy", "L", 7), ("Navy", "XL", 4), ("White", "S", 20), ("White", "M", 22), ("White", "L", 6), ("White", "XL", 1));
                AddProductVariants(db, product19, ("Red", "S", 12), ("Red", "M", 33), ("Red", "L", 2), ("Red", "XL", 5), ("Blue", "S", 12), ("Blue", "M", 1), ("Blue", "L", 4), ("Blue", "XL", 0));
                AddProductVariants(db, product20, ("Purple", "S", 12), ("Purple", "M", 10), ("Purple", "L", 8), ("Purple", "XL", 0), ("Black", "S", 2), ("Black", "M", 10), ("Black", "L", 3), ("Black", "XL", 7));
                AddProductVariants(db, product17, ("Gray", "S", 10), ("Gray", "M", 20), ("Gray", "L", 4), ("Gray", "XL", 6), ("Brown", "S", 8), ("Brown", "M", 15), ("Brown", "L", 5), ("Brown", "XL", 3));
                AddProductVariants(db, product18, ("Navy", "S", 18), ("Navy", "M", 25), ("Navy", "L", 7), ("Navy", "XL", 4), ("White", "S", 20), ("White", "M", 22), ("White", "L", 6), ("White", "XL", 1));
                AddProductVariants(db, product19, ("Red", "S", 12), ("Red", "M", 33), ("Red", "L", 2), ("Red", "XL", 5), ("Blue", "S", 12), ("Blue", "M", 1), ("Blue", "L", 4), ("Blue", "XL", 0));
                AddProductVariants(db, product20, ("Purple", "S", 12), ("Purple", "M", 10), ("Purple", "L", 8), ("Purple", "XL", 0), ("Black", "S", 2), ("Black", "M", 10), ("Black", "L", 3), ("Black", "XL", 7));
                var colours = new[] { "Red", "Blue", "Green", "Black", "Gray", "Navy", "White", "Brown", "Purple"};
                db.AddRange(product1, product2, product3, product4, product5, product6, product7, product8, product9, product10,
                    product11, product12, product13, product14, product15, product16, product17, product18, product19, product20);

                db.SaveChanges();
            }
        }

        private static void AddProductVariants(MyDbContext db, Product product, params (string Color, string Size, int Quantity)[] variants)
        {
            var colors = db.Colours.Where(c => variants.Select(v => v.Color).Contains(c.ColourName)).ToList();
            var sizes = db.Sizes.Where(s => variants.Select(v => v.Size).Contains(s.SizeName)).ToList();

            product.ProductVariants = variants
                .Select(v => new ProductVariant
                {
                    Colour = colors.First(c => c.ColourName == v.Color),
                    Size = sizes.First(s => s.SizeName == v.Size),
                    Quantity = v.Quantity
                })
                .ToList();
        }

        private static Product CreateProduct(string name, string description, double price, bool featured, int supplierId, string[] categoryNames, MyDbContext db)
        {
            var categories = new List<Category>();

            foreach (var categoryName in categoryNames)
            {
                var category = db.Categories.FirstOrDefault(c => c.CategoryName == categoryName);

                if (category == null)
                {
                    category = new Category { CategoryName = categoryName };
                    db.Categories.Add(category);
                    db.SaveChanges();
                }

                categories.Add(category);
            }

            return new Product
            {
                Name = name,
                Description = description,
                Price = price,
                Categories = categories,
                ProductSupplierId = supplierId,
                FeaturedProduct = featured
            };
        }
    }
}
