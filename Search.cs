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
            List<Product> searchResults;
            while (!success)
            {
                var userInput = InputHelpers.GetInput("Search: ");

                searchResults = db.Products
                    .Where(x => x.Name.Contains(userInput) || x.Categories.Any(c => c.CategoryName.Contains(userInput) && (c.CategoryName.StartsWith(userInput))))
                    .ToList();


                if (searchResults.Count == 0)
                {
                    Console.WriteLine("No products found! Try again...");
                }
                else
                {
                    Console.WriteLine("Search results:");

                    for (int i = 0; i < searchResults.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {searchResults[i].Name}");
                    }

                    var selectedIndex = InputHelpers.GetIntegerInput("Select the correct product or enter 0 to search again: ");

                    if (selectedIndex == 0)
                    {
                        continue; 
                    }

                    var selectedProduct = searchResults[selectedIndex - 1];

                    var showProduct = InputHelpers.GetYesOrNo($"Correct product: {selectedProduct.Name} ");
                    if (showProduct)
                    {
                        var basket = ShowProductFromSearch(selectedProduct);
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
                    Console.WriteLine("Available Variants in stock:");

<<<<<<< HEAD

=======
>>>>>>> 3c71aa108145a6ccba78292207b5a9a50ffad42c
                    Console.Write("Size: ");
                    var sizes = productVariants.Select(variant => variant.Size?.SizeName ?? "N/A").Distinct();
                    Console.WriteLine(string.Join(", ", sizes));

                    Console.Write("Colour: ");
                    var colors = productVariants.Select(variant => variant.Colour?.ColourName ?? "N/A").Distinct();
                    Console.WriteLine(string.Join(", ", colors));

<<<<<<< HEAD

=======
>>>>>>> 3c71aa108145a6ccba78292207b5a9a50ffad42c
                    foreach (var variant in productVariants)
                    {
                        //// Kommenterade bort quantity, ska det vara kvar? känns mer som en admin feature än vad kunderna behöver se när de söker på en produkt? , Quantity: {variant.Quantity}
                        Console.WriteLine($"- Size: {variant.Size?.SizeName ?? "N/A"}, Color: {variant.Colour?.ColourName ?? "N/A"}");
                    }
<<<<<<< HEAD

=======
>>>>>>> 3c71aa108145a6ccba78292207b5a9a50ffad42c

                    var basket = BasketHelpers.AddProductToBasket(product);
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


        public static ProductOrder CategorySearch()
        {
            using var db = new MyDbContext();

            var categories = db.Categories.ToList();
            bool success = false;
            ProductOrder basket = null;

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

                        var specificProduct = specificProducts.FirstOrDefault(x => x.Id == selectedProduct);

                        if (specificProduct != null)
                        {
                            Console.WriteLine($"Selected Product {specificProduct.Id}");
                            basket = ShowProductFromSearch(specificProduct);
                            success = true;
                        }
                        else
                        {
                            Console.WriteLine("Invalid Product_Id. No product found.");
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

            return basket;
        }
    }
}