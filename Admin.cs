using Microsoft.EntityFrameworkCore;
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
<<<<<<< HEAD
                    case MyEnums.AdminMenu.RemoveProduct: break;
                    case MyEnums.AdminMenu.ChangeProduct: ChangeProduct(); break;
                    case MyEnums.AdminMenu.ChangeFeatured: ManageFeaturedProduct(); break;
=======
                    case MyEnums.AdminMenu.RemoveProduct: RemoveProduct(); break;
                    case MyEnums.AdminMenu.ChangeProduct: break;
>>>>>>> 3c71aa108145a6ccba78292207b5a9a50ffad42c
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
<<<<<<< HEAD

        public static void ChangeProduct()
        {
            using var db = new MyDbContext();
            Console.WriteLine();

            Console.WriteLine("List of existing products: ");
            var existingProducts = db.Products.Include(p => p.ProductVariants).ToList();

            foreach (var product in existingProducts)
            {
                Console.WriteLine($"Id: {product.Id}, Name: {product.Name}, Description: {product.Description}, Price: {product.Price}, SupplierId: {product.ProductSupplierId}, Featured: {product.FeaturedProduct}");

                var distinctColours = product.ProductVariants.Select(pv => db.Colours.FirstOrDefault(c => c.Id == pv.ColourId)?.ColourName).Distinct().Where(c => c != null);
                var distinctSizes = product.ProductVariants.Select(pv => db.Sizes.FirstOrDefault(s => s.Id == pv.SizeId)?.SizeName).Distinct().Where(s => s != null);

                Console.WriteLine("Colours:");
                Console.WriteLine(string.Join(", ", distinctColours));

                Console.WriteLine("Sizes:");
                Console.WriteLine(string.Join(", ", distinctSizes));

                Console.WriteLine();
            }

            int productId = InputHelpers.GetIntegerInput("Enter the ID of the product you want to change: ");
            var productToChange = db.Products.FirstOrDefault(p => p.Id == productId);

            if (productToChange != null)
            {
                Console.WriteLine("1. Name");
                Console.WriteLine("2. Description");
                Console.WriteLine("3. Price");
                Console.WriteLine("4. Product supplier");
                Console.WriteLine("5. Featured product");
                Console.WriteLine("6. Colour");
                Console.WriteLine("7. Size");
                Console.WriteLine();

                int chosenOption = InputHelpers.GetIntegerInput("Select what you want to change: ");
                switch (chosenOption)
                {
                    case 1:
                        productToChange.Name = InputHelpers.GetInput("Enter the new name: ");
                        break;
                    case 2:
                        productToChange.Description = InputHelpers.GetInput("Enter the new description: ");
                        break;

                    case 3:
                        productToChange.Price = InputHelpers.GetDoubleInput("Enter the new price: ");
                        break;
                    case 4:
                        productToChange.ProductSupplierId = InputHelpers.GetIntegerInput("Enter the Id of the new supplier: ");
                        break;
                    case 5:
                        productToChange.FeaturedProduct = InputHelpers.GetYesOrNo("Is it a featured product? (Yes/No) ");
                        break;
                    case 6:
                        AddColour(productToChange);
                        break;
                        //case 7:
                        //    ChangeSize(productToChange);
                        //    break;
                }
                db.SaveChanges();
                Console.WriteLine("Product updated!");
            }
            else
            {
                Console.WriteLine("Product not found.");
            }

        }

        public static void AddColour(Product product)
        {
            using var db = new MyDbContext();
            Console.WriteLine();

            Console.WriteLine($"Current colours for {product.Name}: ");
            var currentColours = product.ProductVariants.Select(pv => db.Colours.FirstOrDefault(c => c.Id == pv.ColourId)?.ColourName).Distinct().Where(c => c != null);
            Console.WriteLine(string.Join(", ", currentColours));

            Console.WriteLine();

            Console.WriteLine("All available colours: ");
            var allColours = db.Colours.ToList();

            foreach (var colour in allColours)
            {
                Console.WriteLine($"{colour.Id}, {colour.ColourName}.");
            }

            Console.WriteLine();

            int colourId = InputHelpers.GetIntegerInput("Enter the Id of the colour you want to add: ");
            var newColour = db.Colours.FirstOrDefault(c => c.Id == colourId);

            if (newColour != null)
            {
                foreach (var variant in product.ProductVariants)
                {
                    variant.ColourId = newColour.Id;
                }
                db.SaveChanges();
                Console.WriteLine();
                Console.WriteLine("Colour added!");
            }
            else
            {
                Console.WriteLine("Colour not found.");
            }
        }
        private static void RemoveColor(Product product)
        {
            using var db = new MyDbContext();

            Console.WriteLine("Current colors:");
            var currentColors = db.Colours.ToList();

            foreach (var color in currentColors)
            {
                Console.WriteLine($"{color.Id}, {color.ColourName}.");
            }

            Console.WriteLine();
            int colorIdToRemove = InputHelpers.GetIntegerInput("Enter the Id of the color to remove: ");
            var colorToRemove = db.Colours.FirstOrDefault(c => c.Id == colorIdToRemove);

            if (colorToRemove != null)
            {
                foreach (var variant in product.ProductVariants)
                {
                    if (variant.ColourId == colorToRemove.Id)
                    {
                        variant.ColourId = 0; 
                    }
                }
                db.SaveChanges();
                Console.WriteLine();
                Console.WriteLine("Color removed!");
            }
            else
            {
                Console.WriteLine("Color not found.");
            }
        }

        //public static void AddSize(Product product)
        //{
        //    using var db = new MyDbContext();
        //    Console.WriteLine();

        //    Console.WriteLine($"Current sizes for {product.Name}: ");
        //    var currentSizes = product.ProductVariants.Select(pv => db.Sizes.FirstOrDefault(s => s.Id == pv.SizeId)?.SizeName).Distinct().Where(s => s != null);
        //    Console.WriteLine(string.Join(", ", currentSizes));

        //    Console.WriteLine();

        //    Console.WriteLine("All available colours: ");
        //    var allSizes = db.Sizes.ToList();

        //    foreach (var size in allSizes)
        //    {
        //        Console.WriteLine($"{size.Id}, {size.SizeName}.");
        //    }

        //    Console.WriteLine();

        //    int sizeId = InputHelpers.GetIntegerInput("Enter the Id of the colour you want to add: ");
        //    var newSize = db.Sizes.FirstOrDefault(s => s.Id == sizeId);

        //    if (newSize != null)
        //    {
        //        foreach (var variant in product.ProductVariants)
        //        {
        //            variant.SizeId = newSize.Id;
        //        }
        //        db.SaveChanges();
        //        Console.WriteLine();
        //        Console.WriteLine("Size added!");
        //    }
        //    else
        //    {
        //        Console.WriteLine("Size not found.");
        //    }
        //}

        //private static void RemoveSize(Product product)
        //{
        //    using var db = new MyDbContext();

        //    Console.WriteLine("Current sizes:");
        //    var currentSizes = db.Sizes.ToList();

        //    foreach (var size in currentSizes)
        //    {
        //        Console.WriteLine($"{size.Id}, {size.SizeName}.");
        //    }

        //    Console.WriteLine();
        //    int sizeIdToRemove = InputHelpers.GetIntegerInput("Enter the Id of the size to remove: ");
        //    var sizeToRemove = db.Sizes.FirstOrDefault(s => s.Id == sizeIdToRemove);

        //    if (sizeToRemove != null)
        //    {
        //        foreach (var variant in product.ProductVariants)
        //        {
        //            if (variant.SizeId == sizeToRemove.Id)
        //            {
        //                variant.SizeId = 0;
        //            }
        //        }
        //        db.SaveChanges();
        //        Console.WriteLine();
        //        Console.WriteLine("Size removed!");
        //    }
        //    else
        //    {
        //        Console.WriteLine("Size not found.");
        //    }
        //}

        //public static void ChangeColour(Product product)
        //{
        //    using var db = new MyDbContext();
        //    Console.WriteLine();

        //    Console.WriteLine("Current colours:");
        //    var currentColour = db.Colours.ToList();

        //    foreach (var colour in currentColour)
        //    {
        //        Console.WriteLine($"{colour.Id}, {colour.ColourName}.");
        //    }
        //    Console.WriteLine();
        //    int colourId = InputHelpers.GetIntegerInput("Enter the Id of the colour you want to set: ");
        //    var selectedColour = db.Colours.FirstOrDefault(c => c.Id == colourId);

        //    if (selectedColour != null)
        //    {
        //        foreach (var variant in product.ProductVariants)
        //        {
        //            variant.ColourId = selectedColour.Id;
        //        }
        //        db.SaveChanges();
        //        Console.WriteLine();
        //        Console.WriteLine("Colour updated!");
        //    }
        //    else
        //    {
        //        Console.WriteLine("Colour not found.");
        //    }
        //}
        //public static void ChangeSize(Product product)
        //{
        //    using var db = new MyDbContext();
        //    Console.WriteLine();

        //    Console.WriteLine("Current sizes:");
        //    var currentSizes = db.Sizes.ToList();
        //    foreach (var size in currentSizes)
        //    {
        //        Console.WriteLine($"{size.Id}, {size.SizeName}");
        //    }
        //    Console.WriteLine();
        //    int sizeId = InputHelpers.GetIntegerInput("Enter the Id of the size you want to set: ");
        //    var selectedSizes = db.Sizes.FirstOrDefault(s => s.Id == sizeId);

        //    if (selectedSizes != null)
        //    {
        //        foreach (var variant in product.ProductVariants)
        //        {
        //            variant.SizeId = selectedSizes.Id;
        //        }
        //        Console.WriteLine();
        //        db.SaveChanges();
        //        Console.WriteLine("Size updated!");
        //    }
        //    else
        //    {
        //        Console.WriteLine("Size not found.");
        //    }
        //}

        public static void ManageFeaturedProduct()
        {
            using var db = new MyDbContext();
            Console.WriteLine();
            Console.WriteLine("List of existing products:");
            var existingProducts = db.Products.ToList();

            foreach (var product in existingProducts)
            {
                Console.WriteLine($"{product.Id}, {product.Name} - Featured: {product.FeaturedProduct}");
            }
            Console.WriteLine();
            int productId = InputHelpers.GetIntegerInput("Enter the ID of the product you want to manage: ");
            var selectedProduct = db.Products.FirstOrDefault(p => p.Id == productId);

            if (selectedProduct != null)
            {
                Console.WriteLine($"Current Featured Status for {selectedProduct.Name}: {selectedProduct.FeaturedProduct}");

                bool newFeaturedStatus = InputHelpers.GetYesOrNo("Is it a featured product? (Yes/No) ");
                Console.WriteLine();
                selectedProduct.FeaturedProduct = newFeaturedStatus;
                db.SaveChanges();

                Console.WriteLine("Featured status updated successfully!");
            }
            else
            {
                Console.WriteLine("Product not found.");
=======
        public static void RemoveProduct()
        {
            using (var db = new MyDbContext())
            {
                ShowProductIds(); // Display product IDs before deletion

                bool success = false;

                while (!success)
                {
                    Console.Write("Enter the ID of the product you want to delete: ");
                    int productIdToDelete = InputHelpers.GetIntegerInput("");

                    var productToDelete = db.Products.Find(productIdToDelete);

                    if (productToDelete != null)
                    {
                        Console.WriteLine($"Product Name: {productToDelete.Name}");
                        Console.WriteLine($"Description: {productToDelete.Description}");
                        Console.WriteLine($"Price: {productToDelete.Price}");

                        var confirmDelete = InputHelpers.GetYesOrNo("Are you sure you want to delete this product?");

                        if (confirmDelete)
                        {
                            db.Products.Remove(productToDelete);
                            db.SaveChanges();

                            Console.WriteLine("Product deleted successfully.");
                            success = true;
                        }
                        else
                        {
                            var returnToMenu = InputHelpers.GetYesOrNo("Return to menu?");
                            if (returnToMenu)
                            {
                                success = true;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Product not found. Please enter a valid product ID.");
                    }
                }
            }
        }
        public static void ShowProductIds()
        {
            using (var db = new MyDbContext())
            {
                var products = db.Products.ToList();

                if (products.Any())
                {
                    Console.WriteLine("Available Product IDs:");
                    foreach (var product in products)
                    {
                        Console.WriteLine($"ID: {product.Id} - Product Name: {product.Name}");
                    }
                }
                else
                {
                    Console.WriteLine("No products available in the database.");
                }
>>>>>>> 3c71aa108145a6ccba78292207b5a9a50ffad42c
            }
        }
    }
}

