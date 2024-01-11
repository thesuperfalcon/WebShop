using Microsoft.EntityFrameworkCore;
using System.Drawing;
using WebShop.Models;

namespace WebShop
{
    internal class Search
    {
        public static void SearchFunction()
        {
            using var db = new MyDbContext();

            //var electronicsCategory = new Category { CategoryName = "Electronics" };
            //var clothingCategory = new Category { CategoryName = "Clothing" };

            //var sizeSmall = new Size { SizeName = "Small" };
            //var sizeMedium = new Size { SizeName = "Medium" };

            //var red = new Colour { ColourName = "Red" };
            //var green = new Colour { ColourName = "Green" };

            //db.AddRange(red, green, electronicsCategory, clothingCategory, sizeMedium, sizeSmall);
            //db.SaveChanges();

            //var colourList = new List<Colour> { red, green };

            //var sizeList = new List<Size> { sizeMedium, sizeSmall };

            //var product = new Product()
            //{
            //    Name = "Test123",
            //    Description = "Test again123",
            //    Price = 1.10,
            //    Categories = new List<Category> { electronicsCategory, clothingCategory }
            //};

            //Random rnd = new Random();

            //db.Add(product);
            //db.SaveChanges();

            //foreach (var size in sizeList)
            //{
            //    foreach (var colour in colourList)
            //    {
            //        var productVariant = new ProductVariant()
            //        {
            //            ProductId = 1,
            //            ColourId = colour.Id,
            //            SizeId = size.Id,
            //            Quantity = rnd.Next(1, 27),
            //        };
            //        db.Add(productVariant);
            //        db.SaveChanges();
            //    }
            //}
            //int quantity = 2;

            //var price = db.ProductVariants
            //.Where(x => x.Id == 2)
            //.Select(x => x.Product.Price)
            //.FirstOrDefault();

            //if (price.HasValue)
            //{
            //    var productOrders1 = new ProductOrder()
            //    {
            //        ProductVariantId = 2,
            //        Quantity = quantity,
            //        TotalPrice = (price.Value * quantity),
            //    };

            //    db.Add(productOrders1);
            //    db.SaveChanges();
            //}

            bool success = false;

            while (!success)
            {
                var userInput = InputHelpers.GetInput("Search: ");

                var searchedProduct = db.Products.Where
                (x => x.Name.Contains(userInput) || x.Categories.Any(x => x.CategoryName.Contains(userInput))).FirstOrDefault();


                if (searchedProduct == null)
                {
                    Console.WriteLine("No products found! Try again...");
                }
                else
                {
                    var showProduct = InputHelpers.GetYesOrNo($"Correct product: {searchedProduct.Name} ");
                    if (showProduct == true)
                    {
                        ShowProductFromSearch(searchedProduct);
                        success = true;
                    }
                }
            }
        }
        public static void ShowProductFromSearch(Product product)
        {
            Console.WriteLine($"Product Name: {product.Name}");
            Console.WriteLine($"Description: {product.Description}");
            Console.WriteLine($"Price: {product.Price}");

            using var db = new MyDbContext();

            try
            {
                var productVariants = db.ProductVariants
                    .Where(x => x.ProductId == product.Id)
                    .Include(x => x.Size)
                    .Include(x => x.Colour)
                    .ToList();

                if (productVariants.Any())
                {
                    Console.WriteLine("Available Variants:");

                    foreach (var variant in productVariants)
                    {
                        Console.WriteLine($"- Size: {variant.Size?.SizeName ?? "N/A"}, Color: {variant.Colour?.ColourName ?? "N/A"}, Quantity: {variant.Quantity}");
                    }
                }
                else
                {
                    Console.WriteLine("No variants found for this product.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving product information: {ex.Message}");
            }
        }
        public static void CategorySearch()
        {
            using var db = new MyDbContext();

            var categories = db.Categories.ToList();

            bool success = false;

            while (!success)
            {
                foreach (var category in categories)
                {
                    Console.WriteLine(category.Id + ". " + category.CategoryName);
                }

                var userChoice = InputHelpers.GetIntegerInput("Category_Id: ");

                var selectedCategory = categories.FirstOrDefault(category => category.Id == userChoice);

                if (selectedCategory != null)
                {
                    Console.WriteLine($"Selected Category: {selectedCategory.CategoryName}");

                    Thread.Sleep(100);

                    var specificProducts = db.Products
                       .Where(p => p.Categories.Any(c => c.Id == selectedCategory.Id))
                       .ToList(); ;

                    if (specificProducts.Any())
                    {
                        Console.WriteLine("Products in the selected category:");
                        foreach (var product in specificProducts)
                        {
                            Console.WriteLine($"- {product.Id} Product Name: {product.Name}");
                        }

                        var selectedProduct = InputHelpers.GetIntegerInput("Product_Id: ");

                        var specificProduct = db.Products.Where(x => x.Id == selectedProduct).FirstOrDefault();

                        if (specificProduct != null)
                        {
                            Console.WriteLine($"Selected Product {specificProduct.Id}");
                            Thread.Sleep(100);
                            ShowProductFromSearch(specificProduct);
                            success = true;
                        }
                    }
                    else
                    {
                        Console.WriteLine("No products found in the selected category.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Category_Id. No category found.");
                }
            }
        }
    }
}