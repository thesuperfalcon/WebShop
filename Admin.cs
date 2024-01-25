using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebShop.Models;
using Dapper;
using System.Security.Cryptography.X509Certificates;


namespace WebShop
{
    //Admin-klassen hanterar adminfunktioner i webbshopen, som att lägga till produkter, ändra produkter, visa statistik osv.
    internal class Admin
    {
        private static string connString = "Data Source=DESKTOP-1ASCK61\\SQLEXPRESS;Initial Catalog=WebShop;Integrated Security=True;TrustServerCertificate=true;";

        //Visa adminmenu
        public static void AdminMenu(Customer customer)
        {
            bool success = false;

            while (!success)
            {
                Console.Clear();
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
                        case MyEnums.AdminMenu.Add_new_product: AddProduct(); break;
                        case MyEnums.AdminMenu.Remove_product: RemoveProductOrVariant(); break;
                        case MyEnums.AdminMenu.Change_product: ChangeProduct(); break;
                        case MyEnums.AdminMenu.Show_inventory_balance: Statistics.ShowInventoryBalance(); break;
                        case MyEnums.AdminMenu.Order_history: Statistics.OrderHistory(); break;
                        case MyEnums.AdminMenu.Customer_information: UpdateCustomerInfo(); break;
                        case MyEnums.AdminMenu.Add_new_customer: LoginManager.CreateCustomer(customer); break;
                        case MyEnums.AdminMenu.Show_statistic: Statistics.ShowStatistic(); break;
                        case MyEnums.AdminMenu.Log_Out: success = true; break;
                        case MyEnums.AdminMenu.Exit: Environment.Exit(0); break;
                    }
                }
                else
                {
                    Console.WriteLine("Wrong input: ");
                }
                Console.Clear();
            }
        }

        // Metod för att lägga till en ny produkt i webbshopen
        public static void AddProduct()
        {
            bool success = false;

            while (!success)
            {

                using var db = new MyDbContext();

                // Input för produktdetaljer.
                var productName = InputHelpers.GetInput("\nProduct name: ");
                var productDescription = InputHelpers.GetInput("Product description: ");
                var productPrice = InputHelpers.GetDoubleInput("Product price: ");
                var suppliers = db.ProductSuppliers.ToList();

                foreach (var supplier in suppliers)
                {
                    Console.WriteLine(supplier.Id + ": " + supplier.SupplierName);
                }

                var inputSupplierId = InputHelpers.GetIntegerInput("Supplier Id: ");
                var productSupplier = db.ProductSuppliers.Where(x => x.Id == inputSupplierId).FirstOrDefault();

                var categories = db.Categories.ToList();

                foreach (var category in categories)
                {
                    Console.WriteLine(category.Id + ": " + category.CategoryName);
                }

                Console.Write("\nCategory / Categories (comma-separated): ");
                var categoryNames = Console.ReadLine().Split(',');

                var chosenCategories = new List<Category>();

                foreach (var categoryName in categoryNames)
                {
                    var categoryNameToUpper = InputHelpers.FormatString(categoryName);
                    var category = db.Categories.FirstOrDefault(c => c.CategoryName == categoryNameToUpper);

                    if (category != null)
                    {
                        chosenCategories.Add(category);
                    }
                    else
                    {
                        var addCategory = new Category { CategoryName = categoryNameToUpper };
                        db.Add(addCategory);
                        db.SaveChanges();
                        chosenCategories.Add(addCategory);
                    }
                }

                // Input för om produkten är "featured".
                var featuredProduct = InputHelpers.GetYesOrNo("\nFeatured product?: ");
                Console.Clear();
                Console.WriteLine("Summary:");
                Console.WriteLine();
                Console.WriteLine("Product name: " + productName);
                Console.WriteLine("Product description: " + productDescription);
                Console.WriteLine("Product supplier: " + productSupplier.SupplierName);
                Console.WriteLine("Price: " + productPrice + "$");
                Console.Write("Categories: ");

                foreach (var category in chosenCategories)
                {
                    Console.Write(category.CategoryName + " ");
                }

                Console.WriteLine();

                var addProduct = InputHelpers.GetYesOrNo("\nAdd new product? \n");

                if (addProduct)
                {
                    // Skapa produktobjekt och lägg till det i databasen.
                    var product = new Product()
                    {
                        Name = productName,
                        Description = productDescription,
                        ProductSupplierId = productSupplier.Id,
                        Price = productPrice,
                        Categories = new List<Category>(chosenCategories),
                        FeaturedProduct = featuredProduct
                    };
                    db.Add(product);
                    db.SaveChanges();

                    success = AddProductVariants(product);

                }
                else
                {
                    Console.Write("Press any key to return to the menu...");
                    Console.ReadKey(true);
                    return;
                }
            }
        }

        // Metod för att lägga till produktvarianter till en befintlig produkt.
        public static bool AddProductVariants(Product product)
        {

            Console.WriteLine(product.Id + " " + product.Name);

            using var db = new MyDbContext();

            var colours = db.Colours.ToList();

            var choosenColours = new List<Colour>();

            foreach (var colour in colours)
            {
                Console.WriteLine(colour.Id + ": " + colour.ColourName);

            }
            Console.Write("\nColour / Colours (comma-seperate): ");
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
            Console.WriteLine("\nSize / Sizes (comma-seperate): ");

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
            // Skapa produktvariantobjekt och lägg till dem i databasen.
            List<ProductVariant> variants = new List<ProductVariant>();

            foreach (var sizeVariant in choosenSizes)
            {
                foreach (var colourVaraint in choosenColours)
                {
                    Console.WriteLine(sizeVariant.SizeName + " - " + colourVaraint.ColourName);
                    var amount = InputHelpers.GetIntegerInput("\nQuantity to add: ");

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
                var variantSize = db.Sizes.FirstOrDefault(c => c.Id == variant.SizeId);
                var variantColour = db.Colours.FirstOrDefault(c => c.Id == variant.ColourId);
                Console.WriteLine("Size: " + variantSize.SizeName + ", Color: " + variantColour.ColourName + ", Quantity: " + variant.Quantity);
                db.Add(variant);

            }
            var addVaraints = InputHelpers.GetYesOrNo("\nAdd variants to the new product? ");
            if (addVaraints == true)
            {
                db.SaveChanges();
                Console.WriteLine("Product added, returning to menu.");
                Thread.Sleep(1500);
                Console.Clear();
            }
            else
            {
                Console.WriteLine("Product not added, returning to menu.");
                Thread.Sleep(1500);
                Console.Clear();
            }
            return true;
        }

        // Metod för att ändra en befintlig produkt.
        public static void ChangeProduct()
        {
            using var db = new MyDbContext();
            Console.WriteLine();

            Console.WriteLine("\nList of existing products: ");
            var existingProducts = db.Products.Include(p => p.ProductVariants).ToList();

            foreach (var product in existingProducts)
            {
                var productSupplier = db.ProductSuppliers.FirstOrDefault(ps => ps.Id == product.ProductSupplierId);

                Console.WriteLine($"Id: {product.Id}, Name: {product.Name}, Description: {product.Description}, Price: {product.Price}$, SupplierId: {product.ProductSupplierId}, Supplier name: {productSupplier.SupplierName}, Featured: {product.FeaturedProduct}");

                var distinctColours = product.ProductVariants.Select(pv => db.Colours.FirstOrDefault(c => c.Id == pv.ColourId)?.ColourName).Distinct().Where(c => c != null);
                var distinctSizes = product.ProductVariants.Select(pv => db.Sizes.FirstOrDefault(s => s.Id == pv.SizeId)?.SizeName).Distinct().Where(s => s != null);

                Console.Write("\nColours: ");
                Console.WriteLine(string.Join(", ", distinctColours));

                Console.Write("Sizes: ");
                Console.WriteLine(string.Join(", ", distinctSizes));

                Console.WriteLine();
            }

            int productId = InputHelpers.GetIntegerInput("\nEnter the ID of the product you want to change: ");
            var productToChange = db.Products.FirstOrDefault(p => p.Id == productId);

            if (productToChange != null)
            {
                Console.WriteLine("1. Name");
                Console.WriteLine("2. Description");
                Console.WriteLine("3. Price");
                Console.WriteLine("4. Product supplier");
                Console.WriteLine("5. Featured product");
                Console.WriteLine("6. Change variant-colour");
                Console.WriteLine("7. Add variant");
                Console.WriteLine();

                int chosenOption = InputHelpers.GetIntegerInput("\nSelect what you want to change: ");
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
                        Console.WriteLine("List of suppliers: ");
                        var suppliers = db.ProductSuppliers.ToList();
                        foreach (var supplier in suppliers)
                        {
                            Console.WriteLine($"{supplier.Id}, {supplier.SupplierName}");
                        }
                        Console.WriteLine();
                        productToChange.ProductSupplierId = InputHelpers.GetIntegerInput("Enter the Id of the new supplier: ");
                        break;
                    case 5:
                        productToChange.FeaturedProduct = InputHelpers.GetYesOrNo("Is it a featured product? (Yes/No) ");
                        break;
                    case 6:
                        ChangeVariant(productToChange);
                        break;
                    case 7:
                        AddVariant(productToChange);
                        break;
                }
                db.SaveChanges();
                Console.WriteLine("Product updated!");
            }
            else
            {
                Console.WriteLine("Product not found.");
            }
        }

        // Metod för att ändra färgen på en variant för en specifik produkt.
        private static void ChangeVariant(Product product)
        {
            using var db = new MyDbContext();
            var productVariants = db.ProductVariants
                .Where(x => x.ProductId == product.Id)
                .Include(x => x.Size)
                .Include(x => x.Colour)
                .ToList();

            Console.WriteLine($"Product {product.Id}: {product.Name}");

            if (productVariants.Any())
            {
                var distinctColors = productVariants.Select(variant => variant.Colour).Distinct();

                foreach (var distinctColour in distinctColors)
                {
                    Console.WriteLine($"Distinct Colour {distinctColour.Id}: {distinctColour.ColourName}");

                    Console.Write("Sizes: ");
                    var sizesForDistinctColour = productVariants
                        .Where(v => v.ColourId == distinctColour.Id)
                        .Select(v => v.Size.SizeName)
                        .Distinct();

                    Console.WriteLine(string.Join(", ", sizesForDistinctColour));
                }

                var userInput = InputHelpers.GetIntegerInput("\nEnter the ID of the color to change its variant: ");

                var variantsToChange = productVariants.Where(x => x.ColourId == userInput).ToList();

                if (variantsToChange.Any())
                {
                    var colorsWithoutProductVariants = Helpers.GetColours(product, db);
                    foreach (var color in colorsWithoutProductVariants)
                    {
                        Console.WriteLine(color.Id + " " + color.ColourName);
                    }

                    var newColourId = InputHelpers.GetIntegerInput("\nEnter the ID of the new color: ");
                    var newColour = db.Colours.FirstOrDefault(c => c.Id == newColourId);

                    if (newColour != null)
                    {
                        foreach (var variant in variantsToChange)
                        {
                            variant.ColourId = newColour.Id;
                        }

                        Console.WriteLine("Variants colors changed successfully!");

                        db.SaveChanges();
                    }
                    else
                    {
                        Console.WriteLine("Invalid new color ID. Please select a valid color.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid color ID. Please select a valid color.");
                }
            }
            else
            {
                Console.WriteLine("No variants found for the product.");
            }
        }

        // Metod för att lägga till en ny variant för en befintlig produkt.
        private static void AddVariant(Product product)
        {
            using var db = new MyDbContext();

            var productColors = db.ProductVariants
                .Where(pv => pv.ProductId == product.Id)
                .Select(pv => pv.Colour)
                .ToList();

            var colorsWithoutProductVariants = db.Colours
                .Where(c => !productColors.Contains(c))
                .ToList();

            if (colorsWithoutProductVariants.Count > 0)
            {

                foreach (var color in colorsWithoutProductVariants)
                {
                    Console.WriteLine(color.Id + " " + color.ColourName);
                }
                var input = InputHelpers.GetIntegerInput("Id: ");

                var specificColour = db.Colours.FirstOrDefault(x => x.Id == input);

                var sizes = db.Sizes.ToList();

                foreach (var size in sizes)
                {
                    Console.Write(size.SizeName + " ");
                }
                Console.WriteLine("Choose sizes (type comma between sizes)");
                var sizeChoices = Console.ReadLine().Split(',');

                var sizeChoiceList = new List<Size>();

                foreach (var choice in sizeChoices)
                {
                    var matchingSizes = db.Sizes
                                           .Where(x => x.SizeName.ToLower() == choice.Trim().ToLower())
                                           .FirstOrDefault();

                    if (matchingSizes != null)
                    {
                        sizeChoiceList.Add(matchingSizes);
                    }
                }


                foreach (var size in sizeChoiceList)
                {
                    Console.WriteLine($"Colour: {specificColour.ColourName} Size: {size.SizeName}");
                    var quantity = InputHelpers.GetIntegerInput("Quantity: ");
                    var variant = new ProductVariant()
                    {
                        ProductId = product.Id,
                        ColourId = specificColour.Id,
                        SizeId = size.Id,
                        Quantity = quantity
                    };
                    db.Add(variant);
                }
                db.SaveChanges();
            }
            else
            {

            }
        }

        // Metod för att ta bort en produkt eller produktvariant.
        public static void RemoveProductOrVariant()
        {
            using (var db = new MyDbContext())
            {
                Console.Clear();
                ShowProductIds();

                Console.Write("\nEnter the ID of the product you want to modify: ");

                int productIdToUpdate = InputHelpers.GetIntegerInput("");

                var productToUpdate = db.Products.Include(p => p.ProductVariants).ThenInclude(v => v.Colour).Include(p => p.ProductVariants).ThenInclude(v => v.Size).FirstOrDefault(p => p.Id == productIdToUpdate);

                if (productToUpdate != null)
                {
                    PrintProductDetails(productToUpdate);

                    Console.WriteLine("\n1. Delete Entire Product");
                    Console.WriteLine("2. Delete Specific Variant");
                    Console.Write("Enter your choice: ");

                    int choice = InputHelpers.GetIntegerInput("");


                    switch (choice)
                    {
                        case 1:
                            DeleteEntireProduct(db, productToUpdate);
                            break;

                        case 2:
                            DeleteProductVariant(db, productToUpdate);
                            break;
                        case 0:
                            return;

                        default:
                            Console.WriteLine("Invalid choice. Please enter 1 or 2.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Product not found. Please enter a valid product ID.");

                }
            }
        }

        // Metod för att skriva ut detaljer om en produkt.
        private static void PrintProductDetails(Product product)
        {
            Console.Clear();
            Console.WriteLine($"Product Name: {product.Name}");
            Console.WriteLine($"Description: {product.Description}");
            Console.WriteLine($"Price: {product.Price}$");

            if (product.ProductVariants.Any())
            {
                Console.WriteLine("\nAvailable Product Variants:");
                Console.WriteLine("");
                foreach (var variant in product.ProductVariants)
                {
                    Console.WriteLine($"Variant ID: {variant.Id} - Color: {variant.Colour.ColourName} - Size: {variant.Size.SizeName}");
                    Console.WriteLine("");
                }
            }
            else
            {
                Console.WriteLine("No variants available for this product.");
            }
        }

        // Metod för att ta bort hela produkten.
        private static void DeleteEntireProduct(MyDbContext db, Product productToDelete)
        {
            var confirmDelete = InputHelpers.GetYesOrNo("\nAre you sure you want to delete this product? ");
            if (confirmDelete)
            {
                db.Products.Remove(productToDelete);
                db.SaveChanges();
                Console.WriteLine("Product deleted successfully.");
                Thread.Sleep(2000);
                Console.Clear();
                return;
            }
            else
            {
                var returnToMenu = InputHelpers.GetYesOrNo("Return to menu?");
                if (returnToMenu)
                {
                    Console.WriteLine("Operation canceled. Returning to menu.");
                    return;
                }
            }
        }

        // Metod för att ta bort en specifik produktvariant.
        private static void DeleteProductVariant(MyDbContext db, Product productToUpdate)
        {
            Console.WriteLine("\nAvailable Product Variants:");
            foreach (var variant in productToUpdate.ProductVariants)
            {
                Console.WriteLine($"Variant ID: {variant.Id} - Variant Name: {variant.Product.Name} - Variant color: {variant.Colour.ColourName} - Variant size: {variant.Size.SizeName}");
            }

            Console.Write("\nEnter the ID of the variant you want to delete: ");
            int variantIdToDelete = InputHelpers.GetIntegerInput("");

            var variantToDelete = productToUpdate.ProductVariants.FirstOrDefault(v => v.Id == variantIdToDelete);

            if (variantToDelete != null)
            {
                Console.WriteLine("");
                Console.WriteLine($"Variant Name: {variantToDelete.Product.Name}");
                Console.WriteLine($"Variant Price: {variantToDelete.Colour.ColourName}");
                Console.WriteLine($"Variant Price: {variantToDelete.Size.SizeName}");
                var confirmDelete = InputHelpers.GetYesOrNo("Are you sure you want to delete this variant?");

                if (confirmDelete)
                {
                    productToUpdate.ProductVariants.Remove(variantToDelete);
                    db.SaveChanges();

                    Console.WriteLine("Variant deleted successfully.");
                    Thread.Sleep(1500);
                    Console.Clear();
                    return;
                }
            }

            else
            {
                Console.WriteLine("Variant not found. Please enter a valid variant ID.");
            }
        }

        // Metod för att visa alla tillgängliga produkt-ID.
        public static void ShowProductIds()
        {
            using (var db = new MyDbContext())
            {
                var products = db.Products.ToList();

                if (products.Any())
                {
                    Console.WriteLine("\nAvailable Product IDs:");

                    foreach (var product in products)
                    {
                        Console.WriteLine($"ID: {product.Id} - Product Name: {product.Name}");
                    }
                }
                else
                {
                    Console.WriteLine("No products available in the database.");
                }
            }
        }

        // Metod för att visa information om alla registrerade kunder.
        public static void ShowAllCustomers()
        {
            using (var db = new MyDbContext())
            {
                var customers = db.Customers
                    .Include(c => c.FirstName)
                    .Include(c => c.LastName)
                    .Include(c => c.Adress)
                        .ThenInclude(a => a.City)
                            .ThenInclude(city => city.Country)
                    .ToList();

                Console.WriteLine("\nAll registred customers:");
                Console.WriteLine();
                foreach (var customer in customers)
                {
                    Console.WriteLine($"Customer ID: {customer.Id}");
                    Console.WriteLine($"Name: {customer.FirstName?.Name} {customer.LastName?.Name}");
                    Console.WriteLine($"Phonenumber: {customer.PhoneNumber}");
                    Console.WriteLine($"Email: {customer.Email}");
                    Console.WriteLine($"Password: {customer.Password}");

                    if (customer.Adress != null)
                    {
                        Console.Write($"Address: {customer.Adress.AdressName},");

                        if (customer.Adress.City != null)
                        {
                            Console.Write($"{customer.Adress.City.CityName},");

                            if (customer.Adress.City.Country != null)
                            {
                                Console.Write($"{customer.Adress.City.Country.CountryName}");
                                Console.WriteLine("");
                            }
                            Console.WriteLine("");

                        }
                    }

                }
            }

        }

        // Metod för att uppdatera kundinformation.
        public static void UpdateCustomerInfo()
        {
            using (var db = new MyDbContext())
            {
                int customerIdToUpdate = -1;

                while (true)
                {
                    Console.Clear();

                    if (customerIdToUpdate == -1)
                    {

                        ShowAllCustomers();
                        Console.Write("\nEnter the ID of the customer you want to update (or enter 0 to exit): ");
                        customerIdToUpdate = InputHelpers.GetIntegerInput("");

                        if (customerIdToUpdate == 0)
                        {
                            Console.WriteLine("Exiting customer update and returning to admin menu.");
                            Thread.Sleep(2000);
                            Console.Clear();
                            return;
                        }
                    }

                    var customerToUpdate = db.Customers
                        .Include(c => c.FirstName)
                        .Include(c => c.LastName)
                        .Include(c => c.Adress)
                            .ThenInclude(a => a.City)
                                .ThenInclude(city => city.Country)
                        .FirstOrDefault(c => c.Id == customerIdToUpdate);
                    if (customerToUpdate == null)
                    {
                        Console.Clear();
                        Console.WriteLine("Invalid customer ID. Please enter a valid customer ID.");
                        Console.ReadKey();
                        customerIdToUpdate = -1;

                    }

                    if (customerToUpdate != null)
                    {
                        Console.Clear();
                        Console.WriteLine($"Current Information for Customer ID {customerToUpdate.Id}:");
                        Console.WriteLine($"1. Firstname: {customerToUpdate.FirstName?.Name}");
                        Console.WriteLine($"2. Lastname: {customerToUpdate.LastName?.Name}");
                        Console.WriteLine($"3. Phonenumber: {customerToUpdate.PhoneNumber}");
                        Console.WriteLine($"4. Email: {customerToUpdate.Email}");
                        Console.WriteLine($"5. Password: {customerToUpdate.Password}");
                        Console.WriteLine($"6. Address: {customerToUpdate.Adress?.AdressName}");
                        Console.WriteLine($"7. City: {customerToUpdate.Adress?.City?.CityName}");
                        Console.WriteLine($"8. Country: {customerToUpdate.Adress?.City?.Country?.CountryName}");
                        Console.WriteLine($"9. Role: {(customerToUpdate.IsAdmin == true ? "Admin" : "Customer")}");
                        Console.WriteLine("\nEnter the number corresponding to the information you want to update (or enter 0 to exit):");

                        int option = InputHelpers.GetIntegerInput("");

                        if (option == 0)
                        {
                            Console.WriteLine("Exiting customer update and returning to admin menu");
                            Thread.Sleep(1000);
                            Console.Clear();
                            return;
                        }
                        else
                        {



                            if (option == 0)
                            {
                                Console.WriteLine("Exiting customer update and returning to admin menu");
                                Thread.Sleep(1500);
                                Console.Clear();
                                return;
                            }
                            else
                            {
                                switch (option)
                                {
                                    case 0:
                                        Console.WriteLine("Exiting customer update and returning to admin menu");
                                        Console.Clear();
                                        return;

                                    case 1:
                                        Console.Write("Enter new First Name: ");
                                        string newFirstName = Console.ReadLine();

                                        var existingFirstName = db.FirstName.FirstOrDefault(f => f.Name == newFirstName);

                                        if (existingFirstName != null)
                                        {
                                            customerToUpdate.FirstName = existingFirstName;
                                        }
                                        else
                                        {

                                            var newFirst = new FirstName { Name = newFirstName };
                                            db.FirstName.Add(newFirst);
                                            db.SaveChanges();
                                            customerToUpdate.FirstName = newFirst;
                                        }

                                        break;

                                    case 2:
                                        Console.Write("Enter new Last Name: ");
                                        string newLastName = Console.ReadLine();


                                        var existingLastName = db.LastName.FirstOrDefault(l => l.Name == newLastName);

                                        if (existingLastName != null)
                                        {

                                            customerToUpdate.LastName = existingLastName;
                                        }
                                        else
                                        {

                                            customerToUpdate.LastName = new LastName { Name = newLastName };
                                            db.LastName.Add(customerToUpdate.LastName);
                                        }

                                        break;
                                    case 3:
                                        bool validPhoneNumber = false;

                                        do
                                        {
                                            Console.Write("Enter new phonenumber: ");
                                            string newPhoneNumberInput = Console.ReadLine();

                                            if (int.TryParse(newPhoneNumberInput, out int parsedPhoneNumber))
                                            {
                                                customerToUpdate.PhoneNumber = parsedPhoneNumber;
                                                validPhoneNumber = true;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Invalid input, only numbers are allowed");
                                            }

                                        } while (!validPhoneNumber);
                                        break;

                                    case 4:
                                        Console.Write("Enter new Email: ");
                                        string newEmail = Console.ReadLine();
                                        customerToUpdate.Email = newEmail;
                                        break;

                                    case 5:
                                        Console.Write("Enter new Password: ");
                                        string newPassword = Console.ReadLine();
                                        customerToUpdate.Password = newPassword;
                                        break;

                                    case 6:
                                        Console.Write("Enter new Address: ");
                                        string newAddress = Console.ReadLine();
                                        customerToUpdate.Adress.AdressName = newAddress;
                                        break;
                                    case 7:
                                        Console.Write("Enter new City: ");
                                        string newCity = Console.ReadLine();
                                        customerToUpdate.Adress.City.CityName = newCity;
                                        break;
                                    case 8:
                                        Console.Write("Enter new Country: ");
                                        string newCountry = Console.ReadLine();
                                        customerToUpdate.Adress.City.Country.CountryName = newCountry;
                                        break;
                                    case 9:
                                        var roleChange = InputHelpers.GetYesOrNo("Admin(Yes/No): ");
                                        if (roleChange == true)
                                        {
                                            customerToUpdate.IsAdmin = true;
                                        }
                                        else
                                        {
                                            customerToUpdate.IsAdmin = false;
                                        }
                                        break;


                                    default:
                                        Console.WriteLine("Invalid option. Please enter a valid number.");
                                        break;
                                }
                            }

                            db.SaveChanges();
                            Console.WriteLine("\nCustomer information updated successfully.");

                            Console.Write("\nDo you want to update more data? (yes/no): ");
                            if (Console.ReadLine()?.Trim().ToLower() != "yes")
                            {
                                Console.WriteLine("\nExiting customer update and returning to admin menu.");
                                Thread.Sleep(1000);
                                Console.Clear();
                                return;
                            }
                        }
                    }
                }
            }
        }
    }
}