using Microsoft.EntityFrameworkCore;
using WebShop.Models;

namespace WebShop
{
    internal class Helpers
    {
        public static List<Colour> GetColours(Product product, MyDbContext db)
        {
            var productColors = db.ProductVariants
                .Where(pv => pv.ProductId == product.Id)
                .Select(pv => pv.Colour)
                .ToList();

            var colorsWithoutProductVariants = db.Colours
                .Where(c => !productColors.Contains(c))
                .ToList();

            return colorsWithoutProductVariants;
        }
        public static int GetSizeId(string size, Product product)
        {
            using var db = new MyDbContext();

            var sizeId = product.ProductVariants
                        .Where(x => size == x.Size.SizeName)
                        .Select(x => x.Size.Id).FirstOrDefault();

            return sizeId;
        }
        public static int GetColourId(string colour, Product product)
        {
            using var db = new MyDbContext();

            var colourId = product.ProductVariants
                          .Where(x => colour.Contains(x.Colour.ColourName))
                          .Select(x => x.Id).FirstOrDefault();

            return colourId;
        }
        public static int GetGeneralId()
        {
            return InputHelpers.GetIntegerInput("Id: ");
        }

        public static Country GetCountry(MyDbContext db)
        {
            while (true)
            {
                Console.WriteLine("List of Countries:");
                var allCountries = db.Countries.ToList();

                foreach (var c in allCountries)
                {
                    Console.WriteLine($"{c.Id}. {c.CountryName}");
                }

                Console.Write("Enter the ID of the desired country: ");
                if (int.TryParse(Console.ReadLine(), out int countryId))
                {
                    var selectedCountry = db.Countries.FirstOrDefault(x => x.Id == countryId);
                    return selectedCountry;
                }
                Console.WriteLine("Invalid input. Please try again.");
            }
        }

        public static City GetOrCreateCity(MyDbContext db, string cityName, Country country)
        {
            City newCity = null;

            while (true)
            {
                var similarCity = db.Cities.FirstOrDefault(ct => ct.CityName.Contains(cityName) && ct.CountryId == country.Id);

                if (similarCity != null)
                {
                    bool isSimilarCity = InputHelpers.GetYesOrNo($"Did you mean {similarCity.CityName}? (yes/no): ");

                    if (isSimilarCity)
                    {
                        return similarCity;
                    }
                }

                if (newCity == null)
                {
                    newCity = new City { CityName = cityName, CountryId = country.Id };
                    var addCity = InputHelpers.GetYesOrNo($"Add city {cityName}?: ");
                    if (addCity)
                    {
                        db.Add(newCity);
                        db.SaveChanges();
                        return newCity;
                    }
                }
            }
        }
        public static Adress ShowAdressInformation(MyDbContext db, Customer customer)
        {
            var customerAddressInfo = db.Customers
                .Where(x => x.Id == customer.Id)
                .Select(c => new Adress
                {
                    Id = c.AdressId,
                    AdressName = c.Adress.AdressName,
                    PostalCode = c.Adress.PostalCode,
                    City = new City
                    {
                        CityName = c.Adress.City.CityName,
                        Country = new Country
                        {
                            CountryName = c.Adress.City.Country.CountryName
                        }
                    }
                })
                .FirstOrDefault();

            if (customerAddressInfo != null)
            {
                Console.WriteLine($"Country: {customerAddressInfo.City.Country.CountryName}");
                Console.WriteLine($"City: {customerAddressInfo.City.CityName}");
                Console.WriteLine($"Postal-Code: {customerAddressInfo.PostalCode}");
                Console.WriteLine($"Adress: {customerAddressInfo.AdressName}");
            }

            return customerAddressInfo;
        }

        public static Adress CreateAddress(MyDbContext dbContext)
        {
            Console.WriteLine("Creating a new address:");

            var existingCountry = Helpers.GetCountry(dbContext);

            string cityName = InputHelpers.GetInput("Enter city name: ");
            var existingCity = Helpers.GetOrCreateCity(dbContext, cityName, existingCountry);

            int postalCode = InputHelpers.GetIntegerInput("Enter postal code: ");

            string addressName = InputHelpers.GetInput("Enter address name: ");

            var existingAddress = dbContext.Adresses.FirstOrDefault(a => a.AdressName == addressName && a.CityId == existingCity.Id);

            if (existingAddress != null)
            {
                //Console.WriteLine("Address with the same name and city already exists. Updating the existing address.");

                // Perform update logic here
                existingAddress.PostalCode = postalCode;

                dbContext.SaveChanges();

                Console.WriteLine($"Address '{addressName}' in {existingCity.CityName}, {existingCountry.CountryName} updated successfully.");
                return existingAddress; // Return the updated address
            }

            var newAddress = new Adress
            {
                AdressName = addressName,
                CityId = existingCity.Id,
                PostalCode = postalCode
            };

            dbContext.Adresses.Add(newAddress);
            dbContext.SaveChanges();

            Console.WriteLine($"Address '{addressName}' with Postal Code {postalCode} in {existingCity.CityName}, {existingCountry.CountryName} created successfully.");

            return newAddress;
        }
        //public static Adress CreateAddress(MyDbContext dbContext)
        //{
        //    Console.WriteLine("Creating a new address:");


        //    var existingCountry = Helpers.GetCountry(dbContext);

        //    string cityName = InputHelpers.GetInput("Enter city name: ");
        //    var existingCity = Helpers.GetOrCreateCity(dbContext, cityName, existingCountry);

        //    int postalCode = InputHelpers.GetIntegerInput("Enter postal code: ");

        //    string addressName = InputHelpers.GetInput("Enter address name: ");


        //    var existingAddress = dbContext.Adresses.FirstOrDefault(a => a.AdressName == addressName && a.CityId == existingCity.Id);

        //    if (existingAddress != null)
        //    {
        //        Console.WriteLine("Address with the same name and city already exists. Returning to the menu.");
        //        return null;
        //    }

        //    var newAddress = new Adress
        //    {
        //        AdressName = addressName,
        //        CityId = existingCity.Id,
        //        PostalCode = postalCode
        //    };

        //    dbContext.Adresses.Add(newAddress);
        //    dbContext.SaveChanges();

        //    Console.WriteLine($"Address '{addressName}' with Postal Code {postalCode} in {existingCity.CityName}, {existingCountry.CountryName} created successfully.");

        //    return newAddress;
        //}
        public static double CalculateBasketValue(List<ProductOrder> basket, MyDbContext db)
        {
            double totalBasketPrice = 0.0;

            foreach (var productOrder in basket)
            {
                if (productOrder != null)
                {
                    var productVariant = db.ProductVariants.Find(productOrder.ProductVariantId);

                    if (productVariant != null)
                    {
                        var productPrice = db.ProductVariants
                            .Include(x => x.Product)
                            .Where(x => x.Id == productOrder.ProductVariantId)
                            .Select(x => x.Product.Price)
                            .FirstOrDefault();

                        if (productPrice != null)
                        {
                            double price = (double)productPrice;
                            double totalPrice = price * productOrder.Quantity;

                            totalBasketPrice += Math.Round(totalPrice, 2);
                        }
                        else
                        {
                            Console.WriteLine($"Warning: ProductVariant not found for ProductOrder with ID {productOrder.Id}");
                        }
                    }
                }
            }

            return totalBasketPrice;
        }
        public static Delivery GetOrCreateDelivery(MyDbContext db, DeliveryName deliveryName, DeliveryType deliveryType, Adress address)
        {
            var deliveryNameId = deliveryName.Id;
            var deliveryTypeId = deliveryType.Id;

            var existingDelivery = db.Deliveries
                .Where(x => x.DeliveryTypeId == deliveryTypeId && x.DeliveryNameId == deliveryNameId)
                .FirstOrDefault();

            if (existingDelivery != null)
            {
                var existingDeliveryAdress = db.Deliveries.Where(x => x.Equals(existingDelivery) && x.Adresses.Contains(address)).FirstOrDefault();
                if (existingDeliveryAdress != null)
                {
                    return existingDeliveryAdress;
                }
                else
                {

                    existingDelivery.Adresses.Add(address);
                    db.SaveChanges();
                    return existingDelivery;
                }
            }
            else
            {
                var newDelivery = new Delivery
                {
                    DeliveryNameId = deliveryNameId,
                    DeliveryTypeId = deliveryTypeId,
                };

                db.Deliveries.Add(newDelivery);
                db.SaveChanges();

                newDelivery.Adresses.Add(address);
                db.SaveChanges();

                return newDelivery;
            }
        }

        public static Payment GetOrCreatePayment(MyDbContext db, PaymentName paymentName, PaymentType paymentType)
        {
            var existingPayment = db.Payments
                .Include(p => p.PaymentName)
                .Include(p => p.PaymentType)
                .FirstOrDefault(p => p.PaymentName == paymentName && p.PaymentType == paymentType);

            if (existingPayment != null)
            {
                return existingPayment;
            }
            else
            {
                var newPayment = new Payment
                {
                    PaymentName = paymentName,
                    PaymentType = paymentType
                };

                db.Payments.Add(newPayment);
                db.SaveChanges();

                return newPayment;
            }
        }
    }
}
