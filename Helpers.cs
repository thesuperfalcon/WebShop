using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebShop.Models;

namespace WebShop
{
    internal class Helpers
    {
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
                Console.WriteLine("Address with the same name and city already exists. Returning to the menu.");
                return null;
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
    }
}
