using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WebShop.Models.Size;

namespace WebShop.Models
{
    public partial class ProductVariant
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ColourId { get; set; }
        public int SizeId { get; set; }
        public int Quantity { get; set; }
        public virtual Product Product { get; set; }
        public virtual Colour Colour { get; set; }
        public virtual Size Size { get; set; }
    }
    public partial class Product
    {
        public Product()
        {
            ProductVariants = new HashSet<ProductVariant>();
        }
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double? Price { get; set; }
        public int? Amount { get; set; }
        public int? ProductSupplierId { get; set; }
        public bool FeaturedProduct { get; set; }
        public virtual ProductSupplier? ProductSupplier { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<ProductVariant> ProductVariants { get; set; }
    }
    public partial class Size
    {
        public Size()
        {
            ProductVariants = new HashSet<ProductVariant>();
        }
        public int Id { get; set; }
        public string SizeName { get; set; }
        public virtual ICollection<ProductVariant> ProductVariants { get; set; }
    }
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
    public partial class Colour
    {
        public Colour()
        {
            ProductVariants = new HashSet<ProductVariant>();
        }
        public int Id { get; set; }
        public string ColourName { get; set; }
        public virtual ICollection<ProductVariant> ProductVariants { get; set; }
    }
    public partial class ProductSupplier
    {
        public ProductSupplier()
        {
            Products = new HashSet<Product>();
        }
        public int Id { get; set; }
        public string SupplierName { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
