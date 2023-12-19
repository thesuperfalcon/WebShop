using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WebShop.Models.Size;

namespace WebShop.Models
{
    public partial class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double? Price { get; set; }
        public int? Amount { get; set; }
        public int SizeId { get; set; }
        public int CategoryId { get; set; }
        public int ColourId { get; set; }
        public int ProductSupplierId { get; set; }
        public bool FeaturedProduct { get; set; }

        public virtual Size? Size { get; set; }
        public virtual Category? Category { get; set; }
        public virtual Colour? Colour { get; set; }
        public virtual ProductSupplier? ProductSupplier { get; set; }
    }
    public partial class Size
    {
        public Size()
        {
            Products = new HashSet<Product>();
        }
        public int Id { get; set; }
        public string SizeName { get; set; }
        public ICollection<Product> Products { get; set; }
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
            Products = new HashSet<Product>();
        }
        public int Id { get; set; }
        public string ColourName { get; set; }
        public virtual ICollection<Product> Products { get; set; }
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
