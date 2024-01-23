using Microsoft.EntityFrameworkCore;

namespace WebShop.Models
{
    internal class MyDbContext : DbContext
    {
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Colour> Colours { get; set; }
        public DbSet<ProductSupplier> ProductSuppliers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<FirstName> FirstName { get; set; }
        public DbSet<LastName> LastName { get; set; }
        public DbSet<Adress> Adresses { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<ProductOrder> ProductOrders { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<DeliveryType> DeliveryTypes { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<DeliveryName> DeliveryNames { get; set; }
        public DbSet<PaymentName> PaymentNames { get; set; }
        public DbSet<FinalOrder> FinalOrders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=WebShopGruppTest123;Trusted_Connection=True;TrustServerCertificate=True;");
            optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=WebShopTestABC;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}