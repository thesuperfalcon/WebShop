using Microsoft.EntityFrameworkCore;
using WebShop.Models;

namespace WebShop
{
    internal class Search
    {
        // Hanterar sökfunktionen för produkter och visar resultatet för användaren.
        public static ProductOrder SearchFunction()
        {
            using var db = new MyDbContext();

            bool success = false;
            List<Product> searchResults;

            while (!success)
            {
                Console.SetCursorPosition(0, 27);
                var userInput = InputHelpers.GetInput("Search: ");

                // Utför sökning baserat på produktnamn och kategorinamn.
                searchResults = db.Products
                    .Where(x => x.Name.Contains(userInput) || x.Categories.Any(c => c.CategoryName.Contains(userInput) && (c.CategoryName.StartsWith(userInput))))
                    .ToList();

                if (searchResults.Count == 0)
                {
                    Console.WriteLine("No products found! Try again...");
                }
                else
                {

                    Console.WriteLine("\nSearch results:");

                    for (int i = 0; i < searchResults.Count; i++)
                    {

                        Console.WriteLine($"{i + 1}. {searchResults[i].Name}");
                    }

                    var selectedIndex = InputHelpers.GetIntegerInput("\nSelect the correct product or enter 0 to search again: ");

                    if (selectedIndex == 0)
                    {
                        continue;
                    }

                    var selectedProduct = searchResults[selectedIndex - 1];

                    // Visar detaljer om den valda produkten och ger användaren möjlighet att lägga till den i varukorgen.
                    var showProduct = InputHelpers.GetYesOrNo($"\nCorrect product: {selectedProduct.Name} ");
                    if (showProduct)
                    {
                        var basket = ShowProductFromSearch(selectedProduct);
                        return basket;
                    }
                }
            }
            return null;
        }

        // Visar detaljer om en specifik produkt och ger användaren möjlighet att lägga till den i varukorgen.
        public static ProductOrder ShowProductFromSearch(Product product)
        {
            Console.WriteLine("\n----Product info----");
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
                    Console.WriteLine("\nAvailable Variants in stock:");

                    Console.Write("Size: ");
                    var sizes = productVariants.Select(variant => variant.Size?.SizeName ?? "N/A").Distinct();
                    Console.WriteLine(string.Join(", ", sizes));

                    Console.Write("Colour: ");
                    var colors = productVariants.Select(variant => variant.Colour?.ColourName ?? "N/A").Distinct();
                    Console.WriteLine(string.Join(", ", colors));

                    foreach (var variant in productVariants)
                    {
                        Console.WriteLine($"Size: {variant.Size?.SizeName ?? "N/A"}, Color: {variant.Colour?.ColourName ?? "N/A"}");
                    }

                    // Lägger till produkten i varukorgen och returnerar varukorgen
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

        // Hanterar sökfunktionen för produkter baserat på kategorier och visar resultatet för användaren.
        public static ProductOrder CategorySearch()
        {
            using var db = new MyDbContext();

            var categories = db.Categories.ToList();
            bool success = false;
            ProductOrder basket = null;

            Console.SetCursorPosition(0, 27);

            while (!success)
            {
                foreach (var category in categories)
                {
                    Console.WriteLine($"{category.Id}. {category.CategoryName}");
                }

                var userChoice = InputHelpers.GetIntegerInput("\nChoose category ID: ");

                var selectedCategory = categories.FirstOrDefault(category => category.Id == userChoice);

                if (selectedCategory != null)
                {
                    Console.WriteLine($"Selected Category: {selectedCategory.CategoryName}");

                    var specificProducts = db.Products
                        .Where(p => p.Categories.Any(c => c.Id == selectedCategory.Id))
                        .ToList();

                    if (specificProducts.Any())
                    {
                        Console.WriteLine("\nProducts in the selected category:");
                        foreach (var product in specificProducts)
                        {
                            Console.WriteLine($"- {product.Id} Product Name: {product.Name}");
                        }

                        var selectedProduct = InputHelpers.GetIntegerInput("Product_Id: ");

                        var specificProduct = specificProducts.FirstOrDefault(x => x.Id == selectedProduct);

                        if (specificProduct != null)
                        {
                            Console.WriteLine($"\nSelected Product {specificProduct.Id}");
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