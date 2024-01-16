using Microsoft.EntityFrameworkCore.Infrastructure;
using WebShop.Models;

namespace WebShop
{
    internal class AddData
    {
        private static string[] Categories = { "Men", "Women", "Pants", "T-Shirt", "Hoodie", "Jeans", "Jacket", "Sweater", "Dress" };
        private static string[] Sizes = { "S", "M", "L", "XL" };
        private static string[] Suppliers = { "Cocktailorde", "Dressman", "Gucci" };
        private static string[] Colours = { "Red", "Blue", "Green", "Black", "Gray", "Navy", "White", "Brown", "Purple", "Yellow" };
        private static string[] DeliveryCompanies = { "Postnord", "Bing", "Dhl" };
        private static string[] DeliveryTypes = { "Home-Delivery", "Pick-Up at nearest shop" };
        private static string[] PaymentCompanies = { "Klarna", "Visa", "Paypal" };
        private static string[] PaymentTypes = { "30-days", "Direct-Payment" };


        public static void RunAddDataMethods()
        {
            try
            {
                AddCountries();
                AddProductInfo();
                AddDeliveryAndPaymentInfo();
                AddCustomerInfo();
                AddMultipleProducts();
                //AddFirstCustomer();  Kolla på denna
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while running AddData methods: {ex.Message}");
            }
        }

        private static void AddProductInfo()
        {
            using var db = new MyDbContext();

            foreach (var categoryName in Categories)
                db.Categories.Add(new Category { CategoryName = categoryName });

            foreach (var sizeName in Sizes)
                db.Sizes.Add(new Size { SizeName = sizeName });

            foreach (var supplierName in Suppliers)
                db.ProductSuppliers.Add(new ProductSupplier { SupplierName = supplierName });

            foreach (var colourName in Colours)
                db.Colours.Add(new Colour { ColourName = colourName });

            db.SaveChanges();
        }
        private static void AddDeliveryAndPaymentInfo()
        {
            Random random = new Random();

            using var db = new MyDbContext();

            foreach (var companyName in DeliveryCompanies)
            {
                db.DeliveryNames.Add(new DeliveryName { Name = companyName });
            }

            foreach (var deliveryType in DeliveryTypes)
            {
                double price = Math.Round(random.Next(100, 180) + random.NextDouble(), 2);
                db.DeliveryTypes.Add(new DeliveryType { DeliveryName = deliveryType, DeliveryPrice = price });
            }

            foreach (var companyName in PaymentCompanies)
            {
                db.PaymentNames.Add(new PaymentName { Name = companyName });
            }

            foreach (var paymentType in PaymentTypes)
            {
                db.PaymentTypes.Add(new PaymentType { PaymentTypeName = paymentType });
            }

            db.SaveChanges();

        }
        private static void AddCustomerInfo()
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
                    { "Oslo", countryName == "Norway" ? country : null }
                };

                foreach (var (cityName, countryEntity) in cities.Where(x => x.Value != null))
                {
                    var city = new City { CityName = cityName, Country = countryEntity };
                    db.Cities.Add(city);
                }
            }

            var addresses = new Dictionary<string, string>
            {
                { "Kungsgatan 21", "Stockholm" },
                { "Storgatan 42", "Göteborg" },
                { "Bränntorp 1", "Stockholm" },
                { "Skogsvägen 19", "Oslo" },
                { "Drottninggatan 89", "Uppsala" },
                { "Nyköpingsvägen 10", "Nyköping" }
            };

            foreach (var (addressName, cityName) in addresses)
            {
                var city = db.Cities.FirstOrDefault(c => c.CityName == cityName);
                if (city != null)
                {
                    var address = new Adress { AdressName = addressName, City = city };
                    db.Adresses.Add(address);
                }
            }

            var firstNames = new[] { "Jens", "Maria", "Pär", "Johanna", "Kalle" };
            var lastNames = new[] { "Svensson", "Göransson", "Eklund", "Karlsson", "Stridh" };

            foreach (var firstName in firstNames)
                db.FirstName.Add(new FirstName { Name = firstName });

            foreach (var lastName in lastNames)
                db.LastName.Add(new LastName { Name = lastName });

            db.SaveChanges();
        }


        // Kolla på denna 

        //private static void AddFirstCustomer()
        //{
        //    using var db = new MyDbContext();


        //    var firstNameJens = db.FirstName.FirstOrDefault(f => f.Name == "Jens") ?? new FirstName { Name = "Jens" };
        //    var lastNameSvensson = db.LastName.FirstOrDefault(l => l.Name == "Svensson") ?? new LastName { Name = "Svensson" };
        //    var addressKungsgatan = db.Adresses.FirstOrDefault(a => a.AdressName == "Kungsgatan 21") ?? new Adress { AdressName = "Kungsgatan 21" };


        //    var firstCustomer = new Customer
        //    {
        //        FirstNameId = firstNameJens.Id,
        //        LastNameId = lastNameSvensson.Id,
        //        AdressId = addressKungsgatan.Id,
        //        PhoneNumber = 123456789,
        //        Email = "jens.svensson@gmail.com",
        //        Password = "123",
        //        IsAdmin = false,
        //    };

        //    // Add and save the first customer
        //    db.Customers.Add(firstCustomer);
        //    db.SaveChanges();
        //}
        private static void AddMultipleProducts()
        {
            using (var db = new MyDbContext())
            {
                var product1 = CreateProduct("Unicorn", "Fancy unicorn shirt", 99.99, false, 3, new[] { "Women", "T-Shirt" }, db);
                var product2 = CreateProduct("Rainbow T-shirt", "Cute T-shirt with rainbows ", 199.99, false, 3, new[] { "Women", "T-Shirt" }, db);
                var product3 = CreateProduct("Hoodie", "Nice hoodie with a cool hood", 49.99, false, 3, new[] { "Women", "Hoodie" }, db);
                var product4 = CreateProduct("Dress", "Cozy dress to look stylish", 79.99, false, 3, new[] { "Women", "Dress" }, db);
                var product5 = CreateProduct("Men's Shirt", "Stylish men's shirt", 59.99, false, 3, new[] { "Men", "Shirt" }, db);
                var product6 = CreateProduct("Men's Jeans", "Comfortable men's jeans", 89.99, false, 3, new[] { "Men", "Jeans", "Pants" }, db);
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
                var product18 = CreateProduct("Slim Fit Men's Jeans", "Modern slim fit jeans for men", 79.99, false, 3, new[] { "Men", "Jeans","Pants" }, db);
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
                var colours = new[] { "Red", "Blue", "Green", "Black", "Gray", "Navy", "White", "Brown", "Purple" };
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

        private static void AddCountries()
        {
            using var db = new MyDbContext();

            var countryNames = new[] { "USA", "China", "India", "Brazil", "Russia", "Japan", "Germany", "United Kingdom", "France", "Italy", "Canada", "Australia", "South Africa", "Mexico", "Spain" };

            foreach (var countryName in countryNames)
            {
                var country = new Country { CountryName = countryName };
                db.Add(country);
            }
            db.SaveChanges();
        }

        private static Product CreateProduct(string name, string description, double price, bool featured, int supplierId, string[] categoryNames, MyDbContext db)
        {
            var categories = categoryNames
                .Select(categoryName => db.Categories.FirstOrDefault(c => c.CategoryName == categoryName) ?? new Category { CategoryName = categoryName })
                .ToList();

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
