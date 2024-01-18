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
                    case MyEnums.AdminMenu.Add_new_product: AddProduct(); break;
                    case MyEnums.AdminMenu.Remove_product: RemoveProductOrVariant(); break;
                    case MyEnums.AdminMenu.Change_product: ChangeProduct(); break;
                    case MyEnums.AdminMenu.Change_featured_product: ManageFeaturedProduct(); break;
                    //case MyEnums.AdminMenu.ChangeProduct: break;
                    case MyEnums.AdminMenu.Show_inventory_balance: break;
                    case MyEnums.AdminMenu.Order_history: break;
                    case MyEnums.AdminMenu.Customer_information: UpdateCustomerInfo(); break;
                    case MyEnums.AdminMenu.Add_new_customer: LoginManager.CreateCustomer(); break;

                    case MyEnums.AdminMenu.Show_statistic: break;
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


        public static void ChangeProduct()
        {
            using var db = new MyDbContext();
            Console.WriteLine();

            Console.WriteLine("List of existing products: ");
            var existingProducts = db.Products.Include(p => p.ProductVariants).ToList();

            foreach (var product in existingProducts)
            {
                Console.WriteLine($"Id: {product.Id}, Name: {product.Name}, Description: {product.Description}, Price: {product.Price}$, SupplierId: {product.ProductSupplierId}, Featured: {product.FeaturedProduct}");

                var distinctColours = product.ProductVariants.Select(pv => db.Colours.FirstOrDefault(c => c.Id == pv.ColourId)?.ColourName).Distinct().Where(c => c != null);
                var distinctSizes = product.ProductVariants.Select(pv => db.Sizes.FirstOrDefault(s => s.Id == pv.SizeId)?.SizeName).Distinct().Where(s => s != null);

                Console.Write("Colours: ");
                Console.WriteLine(string.Join(", ", distinctColours));

                Console.Write("Sizes: ");
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
                Console.WriteLine("7. Add-Variant");
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
            }
        }
        public static void RemoveProductOrVariant()
        {
            using (var db = new MyDbContext())
            {
                Console.Clear();
                ShowProductIds();

                Console.Write("Enter the ID of the product you want to modify: ");
                
                int productIdToUpdate = InputHelpers.GetIntegerInput("");

                var productToUpdate = db.Products.Include(p => p.ProductVariants).ThenInclude(v => v.Colour).Include(p => p.ProductVariants).ThenInclude(v => v.Size).FirstOrDefault(p => p.Id == productIdToUpdate);

                if (productToUpdate != null)
                {
                    PrintProductDetails(productToUpdate);

                    Console.WriteLine("1. Delete Entire Product");
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
                        case 0: AdminMenu();
                            break;

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

        private static void PrintProductDetails(Product product)
        {
            Console.Clear();
            Console.WriteLine($"Product Name: {product.Name}");
            Console.WriteLine($"Description: {product.Description}");
            Console.WriteLine($"Price: {product.Price}$");

            if (product.ProductVariants.Any())
            {
                Console.WriteLine("Available Product Variants:");
                foreach (var variant in product.ProductVariants)
                {
                    Console.WriteLine($"   Variant ID: {variant.Id} - Color: {variant.Colour.ColourName} - Size: {variant.Size.SizeName}");
                }
            }
            else
            {
                Console.WriteLine("No variants available for this product.");
            }
        }

        private static void DeleteEntireProduct(MyDbContext db, Product productToDelete)
        {
            var confirmDelete = InputHelpers.GetYesOrNo("Are you sure you want to delete this product?");
            if (confirmDelete)
            {
                db.Products.Remove(productToDelete);
                db.SaveChanges();
                Console.WriteLine("Product deleted successfully.");
                Thread.Sleep(2000);
                Console.Clear();
                AdminMenu();

            }
            else
            {
                var returnToMenu = InputHelpers.GetYesOrNo("Return to menu?");
                if (returnToMenu)
                {
                    Console.WriteLine("Operation canceled. Returning to menu.");
                    AdminMenu();

                }
            }
        }

        private static void DeleteProductVariant(MyDbContext db, Product productToUpdate)
        {
            Console.WriteLine("Available Product Variants:");
            foreach (var variant in productToUpdate.ProductVariants)
            {
                Console.WriteLine($"Variant ID: {variant.Id} - Variant Name: {variant.Product.Name} - Variant color: {variant.Colour.ColourName} - Variant size: {variant.Size.SizeName}");
            }

            Console.Write("Enter the ID of the variant you want to delete: ");
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
                    AdminMenu();
                }

            }
            else
            {
                Console.WriteLine("Variant not found. Please enter a valid variant ID.");
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
            }
        }
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

                Console.WriteLine("All registred customers:");
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
        public static void UpdateCustomerInfo()
        {
            using (var db = new MyDbContext())
            {
                Console.Clear(); // Clear the console screen
                ShowAllCustomers();

                Console.Write("Enter the ID of the customer you want to update (or enter 0 to exit): ");
                int customerIdToUpdate = InputHelpers.GetIntegerInput("");

                if (customerIdToUpdate == 0)
                {
                    Console.WriteLine("Exiting customer update and returning to admin menu");
                    Thread.Sleep(2000);
                    Console.Clear();
                    AdminMenu();
                }
                else
                {
                    var customerToUpdate = db.Customers
                        .Include(c => c.FirstName)
                        .Include(c => c.LastName)
                        .Include(c => c.Adress)
                            .ThenInclude(a => a.City)
                                .ThenInclude(city => city.Country)
                        .FirstOrDefault(c => c.Id == customerIdToUpdate);

                    if (customerToUpdate != null)
                    {
                        Console.Clear(); // Clear the console screen before updating
                        Console.WriteLine($"Current Information for Customer ID {customerToUpdate.Id}:");
                        Console.WriteLine($"1. Firstname: {customerToUpdate.FirstName?.Name}");
                        Console.WriteLine($"2. Lastname: {customerToUpdate.LastName?.Name}");
                        Console.WriteLine($"3. Phonenumber: {customerToUpdate.PhoneNumber}");
                        Console.WriteLine($"4. Email: {customerToUpdate.Email}");
                        Console.WriteLine($"5. Password: {customerToUpdate.Password}");
                        Console.WriteLine($"6. Address: {customerToUpdate.Adress?.AdressName}");
                        Console.WriteLine($"7. City: {customerToUpdate.Adress?.City?.CityName}");
                        Console.WriteLine($"8. Country: {customerToUpdate.Adress?.City?.Country?.CountryName}");
                        Console.WriteLine("\nEnter the number corresponding to the information you want to update (or enter 0 to exit):");

                        int option = InputHelpers.GetIntegerInput("");

                        if (option == 0)
                        {
                            Console.WriteLine("Exiting customer update and returning to admin menu");
                            Thread.Sleep(1000);
                            Console.Clear();
                            AdminMenu();
                        }
                        else
                        {
                          
                               switch (option)
                            {
                                case 0:
                                    Console.WriteLine("Exiting customer update and returning to admin menu");
                                    Console.Clear();
                                    AdminMenu();
                                    break;

                                case 1:
                                    Console.Write("Enter new First Name: ");
                                    string newFirstName = Console.ReadLine();

                                    // Check if the new name already exists
                                    var existingFirstName = db.FirstName.FirstOrDefault(f => f.Name == newFirstName);

                                    if (existingFirstName != null)
                                    {
                                        // Use the existing FirstName if it already exists
                                        customerToUpdate.FirstName = existingFirstName;
                                    }
                                    else
                                    {
                                        // Create a new FirstName if it doesn't exist
                                        customerToUpdate.FirstName = new FirstName { Name = newFirstName };
                                        db.FirstName.Add(customerToUpdate.FirstName);
                                    }

                                    break;

                                case 2:
                                    Console.Write("Enter new Last Name: ");
                                    string newLastName = Console.ReadLine();

                                    // Check if the new name already exists
                                    var existingLastName = db.LastName.FirstOrDefault(l => l.Name == newLastName);

                                    if (existingLastName != null)
                                    {
                                        // Use the existing LastName if it already exists
                                        customerToUpdate.LastName = existingLastName;
                                    }
                                    else
                                    {
                                        // Create a new LastName if it doesn't exist
                                        customerToUpdate.LastName = new LastName { Name = newLastName };
                                        db.LastName.Add(customerToUpdate.LastName);
                                    }

                                    break;
                                case 3:
                                    Console.Write("Enter new phonenumber: ");
                                    string newPhoneNumber = Console.ReadLine();
                                    customerToUpdate.Email = newPhoneNumber;
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



                                default:
                                    Console.WriteLine("Invalid option. Please enter a valid number.");
                                    break;
                            }
                        }

                            db.SaveChanges();
                            Console.WriteLine("Customer information updated successfully.");

                            Console.Write("Do you want to update more data? (yes/no): ");
                            if (Console.ReadLine()?.Trim().ToLower() != "yes")
                            {
                                Console.WriteLine("Exiting customer update and returning to admin menu.");
                                Thread.Sleep(1000);
                                Console.Clear();
                                AdminMenu();
                            }
                        else
                        {
                            UpdateCustomerInfo();
                        }
                        }                    
                    else
                    {
                        Console.WriteLine("Customer not found. Please enter a valid customer ID.");
                    }
                }
            }
        }
    }
}


