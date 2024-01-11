using Microsoft.EntityFrameworkCore;
using System.Drawing;
using WebShop.Models;

namespace WebShop
{
    internal class Search
    {
        public static ProductOrder SearchFunction()
        {
            using var db = new MyDbContext();

            bool success = false;

            while (!success)
            {
                var userInput = InputHelpers.GetInput("Search: ");

                var searchedProduct = db.Products
                    .Where(x => x.Name.Contains(userInput) || x.Categories.Any(c => c.CategoryName.Contains(userInput)))
                    .FirstOrDefault();

                if (searchedProduct == null)
                {
                    Console.WriteLine("No products found! Try again...");
                }
                else
                {
                    var showProduct = InputHelpers.GetYesOrNo($"Correct product: {searchedProduct.Name} ");
                    if (showProduct)
                    {
                        var basket = ShowProductFromSearch(searchedProduct);
                        return basket;
                    }
                }
            }

            return null;
        }

        public static ProductOrder ShowProductFromSearch(Product product)
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

                    var basket = TheMenu.AddProductToBasket(product);
                    return basket;
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

            return null;
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
                    Console.WriteLine($"{category.Id}. {category.CategoryName}");
                }

                var userChoice = InputHelpers.GetIntegerInput("Category_Id: ");

                var selectedCategory = categories.FirstOrDefault(category => category.Id == userChoice);

                if (selectedCategory != null)
                {
                    Console.WriteLine($"Selected Category: {selectedCategory.CategoryName}");
                    Thread.Sleep(100);

                    var specificProducts = db.Products
                        .Where(p => p.Categories.Any(c => c.Id == selectedCategory.Id))
                        .ToList();

                    if (specificProducts.Any())
                    {
                        Console.WriteLine("Products in the selected category:");
                        foreach (var product in specificProducts)
                        {
                            Console.WriteLine($"- {product.Id} Product Name: {product.Name}");
                        }

                        var selectedProduct = InputHelpers.GetIntegerInput("Product_Id: ");

                        var specificProduct = db.Products.FirstOrDefault(x => x.Id == selectedProduct);

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