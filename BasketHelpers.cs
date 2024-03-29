﻿using Microsoft.EntityFrameworkCore;
using WebShop.Models;
using static WebShop.WindowUI;

namespace WebShop
{
    // BasketHelpers: En samling av hjälpmetoder för att hantera varukorgen i webbshopen.
    internal class BasketHelpers
    {
        // Metod för att lägga till en produkt i varukorgen med val av färg och storlek
        public static ProductOrder AddProductToBasket(Product product)
        {
            using var db = new MyDbContext();

            try
            {
                var addProduct = InputHelpers.GetYesOrNo("\nAdd to cart?: ");

                if (addProduct)
                {
                    var colourChoice = db.ProductVariants
                        .Where(x => x.ProductId == product.Id)
                        .Select(x => x.Colour)
                        .Distinct()
                        .ToList();

                    foreach (var colour in colourChoice)
                    {
                        Console.WriteLine($"{colour.Id}. {colour.ColourName}");
                    }

                    var colourIdInput = InputHelpers.GetIntegerInput("\nEnter colour ID: ");
                    var selectedColour = db.Colours.FirstOrDefault(x => x.Id == colourIdInput);

                    var sizeChoice = db.ProductVariants
                        .Where(x => x.ColourId == selectedColour.Id && x.ProductId == product.Id)
                        .Select(x => x.Size)
                        .Distinct()
                        .ToList();

                    foreach (var size in sizeChoice)
                    {
                        Console.WriteLine($"{size.Id}. {size.SizeName}");
                    }

                    var sizeIdInput = InputHelpers.GetIntegerInput("\nEnter size ID: ");
                    var selectedSize = db.Sizes.FirstOrDefault(x => x.Id == sizeIdInput);

                    var productVariant = db.ProductVariants
                        .Where(x => x.ColourId == selectedColour.Id && x.ProductId == product.Id && x.SizeId == selectedSize.Id)
                        .FirstOrDefault();

                    if (productVariant == null)
                    {
                        Console.WriteLine("Product variant not found.");
                        return null;
                    }

                    Console.WriteLine($"\nAvailable Quantity: {productVariant.Quantity}");

                    var quantity = InputHelpers.GetIntegerInput("Enter the amount you want to order: ");

                    if (quantity >= 0 && quantity <= productVariant.Quantity)
                    {
                        double? productPrice = db.ProductVariants
                            .Where(pv => pv.Id == productVariant.Id)
                            .Select(pv => pv.Product.Price)
                            .FirstOrDefault();

                        double totalPrice = (productPrice * quantity).Value;

                        var addToBasket = InputHelpers.GetYesOrNo("\nAdd to basket?: ");

                        if (addToBasket)
                        {
                            productVariant.Quantity -= quantity;

                            ProductOrder productOrder = new ProductOrder()
                            {
                                ProductVariantId = productVariant.Id,
                                Quantity = quantity,
                                TotalPrice = totalPrice,
                            };

                            db.SaveChanges();
                            return productOrder;
                        }
                    }
                    else if (quantity < 0)
                    {
                        Console.WriteLine("Error: Quantity must be zero or positive.");
                    }
                    else
                    {
                        Console.WriteLine("Error: Selected quantity exceeds available quantity.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return null;
        }


        // Visar detaljer om produkterna i kundvagnen
        public static void DisplayProductDetails(List<ProductOrder> basket, MyDbContext db)
        {
            Console.WriteLine("\n----Product details----");
            foreach (var item in basket)
            {
                var productVariant = GetProductVariant(db, item.ProductVariantId);

                if (productVariant != null)
                {
                    Console.WriteLine($"Product: {productVariant.Product.Name}");
                    Console.WriteLine($"Description: {productVariant.Product.Description}");
                    Console.WriteLine($"Size: {productVariant.Size.SizeName}");
                    Console.WriteLine($"Colour: {productVariant.Colour.ColourName}");
                    Console.WriteLine($"Quantity: {item.Quantity}");
                    Console.WriteLine($"Price: {productVariant.Product.Price}:-");
                }
            }
        }

        // Visar kundvagnens innehåll och ger alternativ för att ändra kvantiteter eller ta bort produkter
        public static void ShowBasket(List<ProductOrder> basket)
        {
            using (var db = new MyDbContext())
            {
                Console.SetCursorPosition(0, 27);
                int number = 1;
                double totalPrice = 0;

                if (basket.Count > 0)
                {
                    List<ProductOrder> validBasketItems = new List<ProductOrder>();

                    DisplayProductDetails(basket, db);
                    var basketTotalValue = Helpers.CalculateBasketValue(basket, db);

                    foreach (var item in basket)
                    {
                        if (item != null)
                        {
                            validBasketItems.Add(item);
                        }

                        Console.WriteLine();
                        number++;
                    }

                    basket.Clear();
                    basket.AddRange(validBasketItems);

                    Console.WriteLine($"Total price: {basketTotalValue}:-");
                    Console.WriteLine();
                    var answer = InputHelpers.GetYesOrNo("Do you want to add more of a product or remove a product? (yes/no): ");

                    if (answer == true)
                    {
                        Console.WriteLine("\n1. Change the quantity of a product.");
                        Console.WriteLine("2. Remove a product from the basket.");

                        int choice;
                        if (int.TryParse(Console.ReadLine(), out choice))
                        {
                            switch (choice)
                            {
                                case 1:
                                    BasketHelpers.UpdateQuantity(basket);
                                    break;
                                case 2:
                                    BasketHelpers.RemoveProduct(basket);
                                    break;
                                default:
                                    Console.WriteLine("Invalid input");
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No products in the basket.");
                }
            }
        }

        // Hämtar en produktvariant baserat på dess ID
        public static ProductVariant GetProductVariant(MyDbContext db, int productVariantId)
        {
            // Hämta en produktvariant från databasen
            var productVariant = db.ProductVariants
                .Include(x => x.Product)
                .Include(x => x.Colour)
                .Include(x => x.Size)
                .FirstOrDefault(x => x.Id == productVariantId);

            return productVariant;
        }

        // Återställer kvantiteterna för produkter i kundvagnen till deras ursprungliga värden
        public static void RollbackQuantities(List<ProductOrder> basket, MyDbContext db)
        {
            foreach (var item in basket)
            {
                var productVariant = GetProductVariant(db, item.ProductVariantId);
                productVariant.Quantity += item.Quantity;
            }

            db.SaveChanges();
        }

        // Visar utvalda produkter som erbjudanden i en särskild gränssnittsfönster
        public static void ShowFeaturedProduct()
        {
            using var db = new MyDbContext();

            var products = db.Products.Where(x => x.FeaturedProduct == true).ToList();

            var featuredProductsWindow = new Window("Extra amazing clothes!", 0, 4, new List<string>());

            foreach (var product in products)
            {

                featuredProductsWindow.TextRows.Add($"  ~ {product.Name} ~");
                featuredProductsWindow.TextRows.Add($"{product.Description}");
                featuredProductsWindow.TextRows.Add($"Price: {product.Price}:-");

                try
                {
                    var productVariants = db.ProductVariants
                        .Where(x => x.ProductId == product.Id)
                        .Include(x => x.Size)
                        .Include(x => x.Colour)
                        .ToList();

                    if (productVariants.Any())
                    {
                        featuredProductsWindow.TextRows.Add("Available Variants:");
                        featuredProductsWindow.TextRows.Add($"Size: {string.Join(", ", productVariants.Select(variant => variant.Size.SizeName).Distinct())}");
                        featuredProductsWindow.TextRows.Add($"Colour: {string.Join(", ", productVariants.Select(variant => variant.Colour.ColourName).Distinct())}");
                    }
                    else
                    {
                        featuredProductsWindow.TextRows.Add("No variants found for this product.");
                    }
                }
                catch (Exception ex)
                {
                    featuredProductsWindow.TextRows.Add($"Error retrieving product information: {ex.Message}");
                }

                featuredProductsWindow.TextRows.Add("");
            }
            featuredProductsWindow.Draw();
        }

        // Uppdaterar kvantiteten för en produkt i kundvagnen
        public static void UpdateQuantity(List<ProductOrder> basket)
        {
            using var db = new MyDbContext();

            DisplayBasket(basket, db);

            if (basket.Count == 0)
            {
                Console.WriteLine("No products in the basket.");
                return;
            }

            int productIdInput = GetProductIdInput();

            if (productIdInput == 0)
            {
                Console.WriteLine("Operation canceled.");
                return;
            }

            var chosenProduct = GetChosenProduct(basket, productIdInput);

            if (chosenProduct == null)
            {
                Console.WriteLine("Product not found in your basket.");
                return;
            }

            DisplayProductQuantityOptions(chosenProduct);

            int option = GetOptionChoice();

            ProcessOption(chosenProduct, option);
        }

        // Tar bort en produkt från kundvagnen
        public static void RemoveProduct(List<ProductOrder> basket)
        {
            using (var db = new MyDbContext())
            {
                foreach (var item in basket)
                {
                    var productVariant = GetProductVariant(db, item.ProductVariantId);
                    Console.WriteLine($"{productVariant.Id} {productVariant.Product.Name}");
                }

                int productVariantId = InputHelpers.GetIntegerInput("\nEnter the ID of the product you want to remove: ");

                var chosenProduct = basket.FirstOrDefault(p => p.ProductVariantId == productVariantId);

                if (chosenProduct != null)
                {
                    var selectedProductVariant = GetProductVariant(db, chosenProduct.ProductVariantId);

                    var removeProduct = InputHelpers.GetYesOrNo($"Are you sure you want to remove {selectedProductVariant.Product.Name} from your basket? (yes/no): ");

                    if (removeProduct)
                    {
                        selectedProductVariant.Quantity += chosenProduct.Quantity;
                        basket.Remove(chosenProduct);
                        Console.WriteLine($"{selectedProductVariant.Product.Name} removed from your basket.");
                        db.SaveChanges();
                    }
                    else
                    {
                        Console.WriteLine("Operation canceled.");
                    }
                }
                else
                {
                    Console.WriteLine("Product not found in your basket.");
                }
            }
        }

        // Privata hjälpmetoder för att hantera kvantitetsuppdateringar och produktborttagning
        private static void DisplayProductQuantityOptions(ProductOrder chosenProduct)
        {
            var chosenProductName = chosenProduct.ProductVariant?.Product?.Name;

            Console.WriteLine($"\nCurrent quantity: {chosenProductName} {chosenProduct.Quantity}");
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. Add quantity");
            Console.WriteLine("2. Remove quantity");
        }

        // Hämtar användarens val för alternativ
        private static int GetOptionChoice()
        {

            int option;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out option))
                {
                    return option;
                }

                Console.WriteLine("Invalid option. Please enter a valid numeric option.");
            }
        }

        // Bearbetar användarens val för alternativ
        private static void ProcessOption(ProductOrder chosenProduct, int option)
        {
            switch (option)
            {
                case 1:
                    AddQuantity(chosenProduct);
                    break;
                case 2:
                    RemoveQuantity(chosenProduct);
                    break;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }

        // Lägg till kvantitet för en produkt i kundvagnen
        private static void AddQuantity(ProductOrder chosenProduct)
        {
            using var db = new MyDbContext();
            Console.WriteLine("\nEnter the quantity you want to add:");

            if (int.TryParse(Console.ReadLine(), out int quantityToAdd))
            {
                chosenProduct.Quantity += quantityToAdd;
                var chosenProductName = chosenProduct.ProductVariant?.Product?.Name;
                Console.WriteLine("Addition completed successfully. Returning to menu.");
                Thread.Sleep(1500);
                UpdateProductVariantQuantity(chosenProduct.ProductVariantId, quantityToAdd, db);
            }
            else
            {
                Console.WriteLine("Invalid quantity.");
            }
        }

        // Tar bort kvantitet för en produkt i kundvagnen
        private static void RemoveQuantity(ProductOrder chosenProduct)
        {
            using var db = new MyDbContext();
            Console.WriteLine("\nEnter the quantity you want to remove:");

            if (int.TryParse(Console.ReadLine(), out int quantityToRemove))
            {
                if (quantityToRemove <= chosenProduct.Quantity)
                {
                    chosenProduct.Quantity -= quantityToRemove;
                    var chosenProductName = chosenProduct.ProductVariant?.Product?.Name;
                    Console.WriteLine("Removal successful. Returning to menu.");
                    Thread.Sleep(1500);
                    UpdateProductVariantQuantity(chosenProduct.ProductVariantId, quantityToRemove, db);
                }
                else
                {
                    Console.WriteLine("Error: The quantity to remove exceeds the current quantity.");
                }
            }
            else
            {
                Console.WriteLine("Invalid quantity.");
            }
        }

        // Uppdatera kvantiteten för en produktvariant i databasen
        private static void UpdateProductVariantQuantity(int productVariantId, int quantityChange, MyDbContext db)
        {
            var productVariant = db.ProductVariants.FirstOrDefault(pv => pv.Id == productVariantId);

            if (productVariant != null)
            {
                productVariant.Quantity += quantityChange;
                db.SaveChanges();
            }
        }

        // Visa innehållet i kundvagnen
        private static void DisplayBasket(List<ProductOrder> basket, MyDbContext db)
        {
            Console.WriteLine("\nProducts in your basket:");

            foreach (var item in basket)
            {
                var productVariant = GetProductVariant(db, item.ProductVariantId);
                Console.WriteLine($"{productVariant.Id}. {productVariant.Product.Name} - Quantity: {item.Quantity}");
            }
        }

        // Hämta en specifik produkt från kundvagnen
        private static ProductOrder GetChosenProduct(List<ProductOrder> basket, int productIdInput)
        {
            return basket.FirstOrDefault(p => p.ProductVariantId == productIdInput);
        }

        // Hämta användarens inmatning för produkt-ID
        private static int GetProductIdInput()
        {
            int productIdInput;
            while (true)
            {
                Console.WriteLine("\nEnter the ID of the product you want to update (or 0 to cancel):");

                if (int.TryParse(Console.ReadLine(), out productIdInput))
                {
                    return productIdInput;
                }

                Console.WriteLine("Invalid input. Please enter a valid product ID.");
            }
        }
    }
}