using Microsoft.Identity.Client;
using WebShop.Models;

namespace WebShop
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //AddFirstCustomer();
            AddFirstOrder();
        }

        public static void AddFirstProduct()
        { 
            using var db = new MyDbContext();
            Category category1 = new Category()
            {
                CategoryName = "T-Shirt"
            };
            db.Add(category1);
            db.SaveChanges();
            ProductSupplier supplier = new ProductSupplier()
            {
                SupplierName = "Gucci"
            };
            db.Add(supplier);
            db.SaveChanges();
            Colour colour = new Colour()
            {
                ColourName = "Red"
            };
            db.Add(colour);
            db.SaveChanges();
            Size size = new Size()
            {
                SizeName = "L",
            };
            db.Add(size);
            db.SaveChanges();
            Product product = new Product()
            {
                Name = "Gucci-Shirt",
                Description = "Is expensive",
                Price = (1999.9),
                Amount = 12,
                SizeId = 1,
                CategoryId = 1, 
                ColourId = 1,
                ProductSupplierId = 1,
                FeaturedProduct = true,
            };
            db.Add(product);
            db.SaveChanges();
        }
        public static void AddFirstCustomer()
        {
            using var db = new MyDbContext();

            Country sweden = new Country()
            {
                CountryName = "Sweden"
            };
            db.Add(sweden);
            db.SaveChanges();
            City nykoping = new City()
            {
                CityName = "Nyköping",
                CountryId = 1,

            };
            db.Add(nykoping); db.SaveChanges();
            Adress adress = new Adress()
            {
                CityId = 1,
                AdressName = "Kungsstigen 23",
                PostalCode = 61214,
            };
            db.Add(adress); db.SaveChanges();
            FirstName erik = new FirstName()
            {
                Name = "Erik"
            };
            db.Add(erik); db.SaveChanges();
            LastName persson = new LastName()
            {
                Name = "Persson",
            };
            db.Add(persson); db.SaveChanges();
            Customer firstCustomer = new Customer()
            {
                FirstNameId = 1,
                LastNameId = 1,
                AdressId = 1,
                PhoneNumber = 0735432189,
                Email = "perssonErik@gmail.com",
                Password = "abc123",
                IsAdmin = false,
            };
            db.Add(firstCustomer); db.SaveChanges();
        }
        public static void AddFirstOrder()
        {
            using var db = new MyDbContext();

            DeliveryType deliveryType = new DeliveryType()
            {
                DeliveryTypeName = "Home-Delivery",
                DeliveryPrice = 119,
            };
            db.Add(deliveryType); db.SaveChanges();

            Delivery delivery = new Delivery()
            {
                DeliveryName = "PostNord",
                DeliveryTypeId = 1,
            };
            db.Add(delivery); db.SaveChanges();
            PaymentType paymentType = new PaymentType()
            {
                PaymentTypeName = "Pay-today"
            };
            db.Add(paymentType); db.SaveChanges();
            Payment payment = new Payment()
            {
                PaymentTypeId = 1,
                PaymentName = "Klarna",
            };
            db.Add(payment); db.SaveChanges();
            Order order = new Order()
            {
                ProductId = 1,
                OrderAmount = 3,
                CustomerId = 1,
                PaymentId = 1,
                DeliveryId = 1,
                TotalPrice = 6118.7,
            };
            db.Add(order); db.SaveChanges();


        }
    }
}