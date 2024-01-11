using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using WebShop.Models;

namespace WebShop
{
    internal class Admin
    {
        public static void AdminMenu()
        {
            Console.WriteLine();

            foreach (int i in Enum.GetValues(typeof(MyEnums.AdminMenu)))
            {
                Console.WriteLine(i + ". " + Enum.GetName(typeof(MyEnums.AdminMenu), i).Replace('_', ' '));
            }
            int nr;
            if (int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out nr))
            {
                MyEnums.AdminMenu menuSelection = (MyEnums.AdminMenu)nr;

                switch (menuSelection)
                {
                    case MyEnums.AdminMenu.AddProduct: AddProduct(); break;
                    case MyEnums.AdminMenu.RemoveProduct: break;
                    case MyEnums.AdminMenu.ChangeProduct: break;
                    case MyEnums.AdminMenu.ShowInventoryBalance: break;
                    case MyEnums.AdminMenu.OrderHistory: break;
                    case MyEnums.AdminMenu.CustomerInformation: break;
                    case MyEnums.AdminMenu.ShowStatistic: break;
                    case MyEnums.AdminMenu.Exit: break;
                }
            }
            else
            {
                Console.WriteLine("Wrong input: ");
            }
            Console.ReadLine();
            Console.Clear();
        }
        public static void AddProduct()
        {
            bool success = false;
            while (!success)
            {
                using var db = new MyDbContext();
                // Product Name
                /* Product Description
                 * Product Price
                 * Product Supplier 
                 * Product Featured
                 * Product Category/Categories
                 */

                var productName = InputHelpers.GetInput("Product_Name: ");

                var productDescription = InputHelpers.GetInput("Product_Description: ");

                var productPrice = InputHelpers.GetDoubleInput("Product_Price: ");

                var suppliers = db.ProductSuppliers.ToList();

                foreach (var supplier in suppliers)
                {
                    Console.WriteLine(supplier.Id + ": " + supplier.SupplierName);
                }

                var inputSupplierId = InputHelpers.GetIntegerInput("Supplier_Id: ");

                var productSupplier = db.ProductSuppliers.Where(x => x.Id == inputSupplierId).FirstOrDefault();

                var categories = db.Categories.ToList();

                foreach (var category in categories)
                {
                    Console.WriteLine(category.Id + ": " + category.CategoryName);
                }

                Console.Write("Category / Categories (comma-separated): ");
                var categoryNames = Console.ReadLine().Split(',');

                var choosenCategories = new List<Category>();

                foreach (var categoryName in categoryNames)
                {

                    var categoryNameToUpper = InputHelpers.FormatString(categoryName);

                    var category = db.Categories.FirstOrDefault(c => c.CategoryName == categoryNameToUpper);

                    if (category != null)
                    {
                        choosenCategories.Add(category);
                    }
                    else
                    {
                        var addCategory = new Category { CategoryName = categoryNameToUpper };
                        db.Add(addCategory);
                        db.SaveChanges();
                        choosenCategories.Add(addCategory);
                    }
                }

                var featuredProduct = InputHelpers.GetYesOrNo("Featured_Product?: ");

                Console.WriteLine(productName);
                Console.WriteLine(productDescription);
                Console.WriteLine(productSupplier.SupplierName);
                Console.WriteLine(productPrice + ":-");
                foreach (var category in choosenCategories)
                {
                    Console.WriteLine(category.CategoryName);
                }

                var addProduct = InputHelpers.GetYesOrNo("Add_Product?: ");

                if (addProduct == true)
                {
                    var product = new Product()
                    {
                        Name = productName,
                        Description = productDescription,
                        ProductSupplierId = productSupplier.Id,
                        Price = productPrice,
                        Categories = new List<Category>(choosenCategories),
                        FeaturedProduct = featuredProduct
                    };
                    db.Add(product);
                    db.SaveChanges();

                    AddProductVariants(product);

                }
                else
                {
                    var returnToMenu = InputHelpers.GetYesOrNo("Return_to_menu?: ");
                    if (returnToMenu == true)
                    {
                        success = true;
                        break;
                    }
                }
            }
        }
        public static void AddProductVariants(Product product)
        {

            Console.WriteLine(product.Id + " " + product.Name);

            using var db = new MyDbContext();

            var colours = db.Colours.ToList();

            var choosenColours = new List<Colour>();

            foreach (var colour in colours)
            {
                Console.WriteLine(colour.Id + ": " + colour.ColourName);

            }
            Console.Write("Colour / Colours (comma-seperate): ");
            var colourNames = Console.ReadLine().Split(',');

            foreach (var colourName in colourNames)
            {

                var colourNameToUpper = InputHelpers.FormatString(colourName);

                var specificColourName = db.Colours.FirstOrDefault(c => c.ColourName == colourNameToUpper);

                if (specificColourName != null)
                {
                    choosenColours.Add(specificColourName);
                }
                else
                {
                    var addColour = new Colour { ColourName = colourNameToUpper };
                    db.Add(addColour);
                    db.SaveChanges();
                    choosenColours.Add(addColour);
                }
            }


            var sizes = db.Sizes.ToList();

            var choosenSizes = new List<Size>();

            foreach (var size in sizes)
            {
                Console.WriteLine(size.SizeName);

            }
            Console.WriteLine("Size / Sizes (comma-seperate): ");

            var sizeNames = Console.ReadLine().Split(',');


            foreach (var sizeName in sizeNames)
            {
                var specificSize = db.Sizes.FirstOrDefault(c => c.SizeName == sizeName);

                if (specificSize != null)
                {
                    choosenSizes.Add(specificSize);
                }
                else
                {

                }
            }

            List<ProductVariant> variants = new List<ProductVariant>();

            foreach (var sizeVariant in choosenSizes)
            {
                foreach (var colourVaraint in choosenColours)
                {
                    Console.WriteLine(sizeVariant.SizeName + " - " + colourVaraint.ColourName);
                    var amount = InputHelpers.GetIntegerInput("Quantity: ");

                    var productVariant = new ProductVariant()
                    {
                        ProductId = product.Id,
                        ColourId = colourVaraint.Id,
                        SizeId = sizeVariant.Id,
                        Quantity = amount,
                    };

                    variants.Add(productVariant);

                }
            }

            Console.Clear();

            foreach (var variant in variants)
            {
                //Console.WriteLine(variant.Id + " " + variant.Product.Name);

                var variantSize = db.Sizes.FirstOrDefault(c => c.Id == variant.SizeId);
                var variantColour = db.Colours.FirstOrDefault(c => c.Id == variant.ColourId);
                Console.WriteLine(variantSize.SizeName);
                Console.WriteLine(variantColour.ColourName);
                Console.WriteLine(variant.Quantity);

                db.Add(variant);

            }
            var addVaraints = InputHelpers.GetYesOrNo("Add_Variants?: ");
            if (addVaraints == true)
            {
                db.SaveChanges();
            }
            else
            {
            }
        }
    }
}

