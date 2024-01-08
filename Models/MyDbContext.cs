using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Models
{
    internal class MyDbContext : DbContext
    {
        public DbSet<Product> Products { get; set;}
        public DbSet<Size> Sizes { get; set;}
        public DbSet<Category> Categories { get; set;}
        public DbSet<Colour> Colours { get; set;}
        public DbSet<ProductSupplier> ProductSuppliers { get; set;}
        public DbSet<Customer> Customers { get; set;}
        public DbSet<FirstName> FirstName { get; set;}
        public DbSet<LastName> LastName { get; set;}
        public DbSet<Adress> Adresses { get; set;}
        public DbSet<City> Cities { get; set;}
        public DbSet<Country> Countries { get; set;}
        public DbSet<FinalOrder> Orders { get; set;}
        public DbSet<Delivery> Deliveries { get; set;}
        public DbSet<DeliveryType> DeliveryTypes { get; set;}
        public DbSet<Payment> Payments { get; set;}
        public DbSet<PaymentType> PaymentTypes { get; set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=WebShopTest1;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}
