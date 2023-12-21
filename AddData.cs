using WebShop.Models;

namespace WebShop
{
    internal class AddData
    {
        public static void AddProductInfo()
        {
            using var db = new MyDbContext();
            {
                var cat1 = new Category() { CategoryName = "Men" };
                var cat2 = new Category() { CategoryName = "Women" };
                var cat3 = new Category() { CategoryName = "Pants" };
                var cat4 = new Category() { CategoryName = "T-Shirt" };
                var cat5 = new Category() { CategoryName = "Hoodie" };

                var size1 = new Size() { SizeName = "S" };
                var size2 = new Size() { SizeName = "M" };
                var size3 = new Size() { SizeName = "L" };
                var size4 = new Size() { SizeName = "XL" };

                var supp1 = new ProductSupplier() { SupplierName = "Cocktailorde" };
                var supp2 = new ProductSupplier() { SupplierName = "Dressman" };
                var supp3 = new ProductSupplier() { SupplierName = "Gucci" };

                var colour1 = new Colour() { ColourName = "Red" };
                var colour2 = new Colour() { ColourName = "Blue" };
                var colour3 = new Colour() { ColourName = "Green" };
                var colour4 = new Colour() { ColourName = "Black" };
                var colour5 = new Colour() { ColourName = "Gray" };

                db.AddRange(cat1, cat2, cat3, cat4, cat5, size1, size2, size3, size4, supp1, supp2, supp3, colour1, colour2, colour3, colour4, colour5);
                db.SaveChanges();
            };
        }
        public static void AddCustomerInfo()
        {
            using var db = new MyDbContext();
            {
                var country1 = new Country() { CountryName = "Sweden" };
                var country2 = new Country() { CountryName = "Norway" };

                var city1 = new City() { CityName = "Nyköping", Country = country1 };
                var city2 = new City() { CityName = "Stockholm", Country = country1 };
                var city3 = new City() { CityName = "Göteborg" , Country = country1 };
                var city4 = new City() { CityName = "Uppsala" , Country = country1 };
                var city5 = new City() { CityName = "Umeå" , Country = country1 };
                var city6 = new City() { CityName = "Oslo" , Country = country2 };

                var adress1 = new Adress() { AdressName = "Kungsgatan 21" , City = city2};
                var adress2 = new Adress() { AdressName = "Storgatan 42" , City = city3};
                var adress3 = new Adress() { AdressName = "Bränntorp 1" , City = city2};
                var adress4 = new Adress() { AdressName = "Skogsvägen 19" , City = city6};
                var adress5 = new Adress() { AdressName = "Drottninggatan 89" , City = city4};
                var adress6 = new Adress() { AdressName = "Nyköpingsvägen 10" , City = city1};

                var firstname1 = new FirstName() { Name = "Jens" };
                var firstname2 = new FirstName() { Name = "Maria" };
                var firstname3 = new FirstName() { Name = "Pär" };
                var firstname4 = new FirstName() { Name = "Johanna" };
                var firstname5 = new FirstName() { Name = "Kalle" };

                var lastname1 = new LastName() { Name = "Svensson" };
                var lastname2 = new LastName() { Name = "Göransson" };
                var lastname3 = new LastName() { Name = "Eklund" };
                var lastname4 = new LastName() { Name = "Karlsson" };
                var lastname5 = new LastName() { Name = "Stridh" };

                db.AddRange(country1, country2, city1, city2, city3, city4, city5, city6, adress1, adress2, adress3, adress4, adress5, adress6,
                    firstname1, firstname2, firstname3, firstname4, firstname5, lastname1, lastname2, lastname3, lastname4, lastname5);
                db.SaveChanges();
            };
        }
        public static void AddOrderInfo()
        {
            using var db = new MyDbContext();
            {
                var deliveryType1 = new DeliveryType() { DeliveryTypeName = "Sending to home", DeliveryPrice = 119 };
                var deliveryType2 = new DeliveryType() { DeliveryTypeName = "Pick-up at nearest store", DeliveryPrice = 79 };

                var delivery1 = new Delivery() { DeliveryName = "DHL" };
                var delivery2 = new Delivery() { DeliveryName = "Postnord" };

                var paymenttype1 = new PaymentType() { PaymentTypeName = "Invoice 30-days" };
                var paymenttype2 = new PaymentType() { PaymentTypeName = "Direct payment" };

                var payment1 = new Payment() { PaymentName = "Klarna" };
                var payment2 = new Payment() { PaymentName = "PayPal" };
                var payment3 = new Payment() { PaymentName = "Credit card" };

                db.AddRange(delivery1, delivery2, deliveryType1, deliveryType2 , payment1, payment2, payment3, paymenttype1, paymenttype2);
                db.SaveChanges();
            }
        }
    }
}