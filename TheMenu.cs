using Microsoft.EntityFrameworkCore;
using WebShop.Models;

namespace WebShop
{
    internal class TheMenu
    {
        private static List<ProductOrder> basket = new List<ProductOrder>();

        public static void ShowMenu(Customer customer)
        {
            bool loop = true;
            while (loop)
            {
                using var db = new MyDbContext();

                ShowFeaturedProduct();

                var productBasket = new ProductOrder();

                foreach (int i in Enum.GetValues(typeof(MyEnums.Menu)))
                {
                    Console.WriteLine(i + ". " + Enum.GetName(typeof(MyEnums.Menu), i).Replace('_', ' '));
                }
                int nr;
                if (int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out nr))
                {
                    MyEnums.Menu menuSelection = (MyEnums.Menu)nr;

                    switch (menuSelection)
                    {
                        case MyEnums.Menu.Search:
                            productBasket = Search.SearchFunction();
                            break;
                        case MyEnums.Menu.Category:
                            ShowCategories();
                            break;
                        case MyEnums.Menu.Cart:
                            ShowBasketTest(basket);
                            break;
                        case MyEnums.Menu.CheckOut:
                            CheckOut(basket);
                            break;
                        case MyEnums.Menu.Exit:
                            loop = false;
                            break;
                    }
                    basket.Add(productBasket);
                }
                else
                {
                    Console.WriteLine("Wrong input: ");
                }

                //ShowBasketTest(basket);

                Console.ReadLine();
                Console.Clear();
            }
        }


        public static void ShowBasketTest(List<ProductOrder> basket)
        {
            using (var db = new MyDbContext())
            {
                int number = 1;
                double totalPrice = 0;

                foreach (var item in basket)
                {
                    Console.WriteLine($"{number}.");

                    var productVariant = db.ProductVariants
                        .Include(x => x.Product)
                        .Include(x => x.Colour)
                        .Include(x => x.Size)
                        .FirstOrDefault(x => x.Id == item.ProductVariantId);

                    if (productVariant != null)
                    {
                        Console.WriteLine($"Product: {productVariant.Product.Name}");
                        Console.WriteLine($"Description: {productVariant.Product.Description}");
                        Console.WriteLine($"Size: {productVariant.Size?.SizeName ?? "N/A"}");
                        Console.WriteLine($"Colour: {productVariant.Colour?.ColourName ?? "N/A"}");
                        Console.WriteLine($"Quantity: {item.Quantity}");
                        Console.WriteLine($"Price: {productVariant.Product.Price ?? 0.0}:-");

                        totalPrice += item.Quantity * (productVariant.Product.Price ?? 0.0);
                    }
                    else
                    {
                        Console.WriteLine("Product variant not found.");
                    }

                    Console.WriteLine();
                    number++;
                }

                Console.WriteLine($"Total price: {totalPrice}:-");
                Console.WriteLine();
                Console.WriteLine("Do you want to add more of a product or remove a product?");

                string answer = Console.ReadLine().ToLower();

                if (answer == "yes")
                {
                    Console.WriteLine("1. Add more quantity of a product.");
                    Console.WriteLine("2. Remove a product from the basket.");

                    int choice;
                    if (int.TryParse(Console.ReadLine(), out choice))
                    {
                        switch (choice)
                        {
                            case 1:
                                AddMoreQuantity(basket);
                                break;
                            case 2:
                                RemoveProduct(basket);
                                break;
                            default:
                                Console.WriteLine("Invalid input");
                                break;
                        }
                    }
                }
            }
        }

        public static void ShowCategories()
        {
            using (var db = new MyDbContext())
            {
                Console.Clear();

                var categories = db.Categories.Where(x => x.Products.Count() > 0).ToList();

                foreach (var category in categories)
                {
                    Console.WriteLine(category.CategoryName);
                }
                Console.Read();
            }
        }

        public static ProductOrder AddProductToBasket(Product product)
        {
            using var Db = new MyDbContext();

            try
            {
                var addProduct = InputHelpers.GetYesOrNo("Add to cart?");
                if (addProduct)
                {
                    var colourChoice = Db.ProductVariants.Where(x => x.ProductId == product.Id).Select(x => x.Colour).ToList();
                    var colourChoiceEtt = colourChoice.Distinct().ToList();

                    foreach (var colour in colourChoiceEtt)
                    {
                        Console.WriteLine(colour.Id + " " + colour.ColourName);
                    }

                    var colourIdInput = InputHelpers.GetIntegerInput("ColourId");
                    var selectedColour = Db.Colours.FirstOrDefault(x => x.Id == colourIdInput);

                    var sizeChoice = Db.ProductVariants
                        .Where(x => x.ColourId == selectedColour.Id && x.ProductId == product.Id)
                        .Select(x => x.Size)
                        .ToList();

                    var sizeChoiceEtt = sizeChoice.Distinct().ToList();
                    foreach (var size in sizeChoiceEtt)
                    {
                        Console.WriteLine(size.Id + " " + size.SizeName);
                    }

                    var sizeIdInput = InputHelpers.GetIntegerInput("SizeId");
                    var selectedSize = Db.Sizes.FirstOrDefault(x => x.Id == sizeIdInput);
                    var quantity = InputHelpers.GetIntegerInput("Amount?");

                    ProductVariant productOrder = Db.ProductVariants
                        .Where(x => x.ColourId == selectedColour.Id && x.ProductId == product.Id && x.SizeId == selectedSize.Id)
                        .FirstOrDefault();

                    if (productOrder == null)
                    {
                        Console.WriteLine("Product variant not found.");
                        return null;
                    }

                    var test = Db.ProductVariants.Where(x => x.Id == productOrder.Id).FirstOrDefault();
                    Console.WriteLine(test.Id);
                    Console.WriteLine(test.ProductId);
                    Console.WriteLine(test.ColourId);
                    Console.WriteLine(test.SizeId);
                    Console.WriteLine(quantity);

                    var addToBasket = InputHelpers.GetYesOrNo("Add to basket?");
                    if (addToBasket)
                    {
                        var selectedProduct = new ProductOrder()
                        {
                            ProductVariantId = test.Id,
                            Quantity = quantity,
                            TotalPrice = 1,
                        };
                        return selectedProduct;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return null;
        }


        public static void ShowBasket(List<ProductOrder> basket)
        {
            //var basket = new List<ProductOrder>();

            using (var db = new MyDbContext())
            {
                int number = 1;
                double totalPrice = 0;
                foreach (var item in basket)
                {
                    Console.WriteLine(number + ".");

                    var product = db.ProductVariants.Include(x => x.Product)
                                        .Include(x => x.Colour)
                                        .Include(x => x.Size)
                                        .FirstOrDefault(x => x.Id == item.ProductVariantId);

                    var colour = db.Colours.Where(x => x.Id == item.ProductVariant.ColourId)
                        .Select(x => x.ColourName).FirstOrDefault();

                    var size = db.Sizes.Where(x => x.Id == item.ProductVariant.SizeId)
                        .Select(x => x.SizeName).FirstOrDefault();

                    Console.WriteLine($"Product: {product.Product.Name}");
                    Console.WriteLine($"Description: {product.Product.Description}");
                    Console.WriteLine($"Size: {size}");
                    Console.WriteLine($"Colour: {colour}");
                    Console.WriteLine($"Quantity: {item.Quantity}");
                    Console.WriteLine($"Price: {item.ProductVariant.Product.Price}:-");
                    totalPrice += item.ProductVariant.Product.Price.Value;
                    number++;
                }
                Console.WriteLine($"Total price: {totalPrice}:-");
                Console.WriteLine();
                Console.WriteLine("Do you want to add more of a product or remove a product?");

                string answer = Console.ReadLine().ToLower();

                if (answer == "yes")
                {
                    Console.WriteLine("1. Add more quantity of a product.");
                    Console.WriteLine("2. Remove a product from the basket.");

                    int choice;
                    if (int.TryParse(Console.ReadLine(), out choice))
                    {
                        switch (choice)
                        {
                            case 1:
                                AddMoreQuantity(basket);
                                break;
                            case 2:
                                RemoveProduct(basket);
                                break;
                            default:
                                Console.WriteLine("Invalid input");
                                break;
                        }
                    }
                }
            }
        }

        public static void AddMoreQuantity(List<ProductOrder> basket)
        {
            //ShowBasket(basket);

            using (var db = new MyDbContext())
            {
                var productName = InputHelpers.GetInput("Enter the name of the product you want to add more of: ");

                var chosenProduct = basket.FirstOrDefault(p => p.ProductVariant.Product.Name == productName);

                if (chosenProduct != null)
                {
                    Console.WriteLine($"Current quantity of {chosenProduct.ProductVariant.Product.Name}: {chosenProduct.Quantity}");
                    Console.WriteLine("Enter the quantity you want to add:");

                    if (int.TryParse(Console.ReadLine(), out int quantityToAdd))
                    {
                        chosenProduct.Quantity += quantityToAdd;

                        Console.WriteLine($"Added {quantityToAdd} more of {chosenProduct.ProductVariant.Product.Name} to your basket.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid quantity.");
                    }
                }
                else
                {
                    Console.WriteLine("Product not found in your basket.");
                }


            }
        }

        public static void RemoveProduct(List<ProductOrder> basket)
        {
            //ShowBasket(basket);

            using (var db = new MyDbContext())
            {
                var productName = InputHelpers.GetInput("Enter the name of the product you want to remove: ");

                var chosenProduct = basket.FirstOrDefault(p => p.ProductVariant.Product.Name == productName.ToLower());
                if (chosenProduct != null)
                {
                    basket.Remove(chosenProduct);
                    Console.WriteLine($"{chosenProduct.ProductVariant.Product.Name} removed from your basket.");
                }
                else
                {
                    Console.WriteLine("Product not found in your basket.");
                }
            }
        }

        //---------------------------Frakt-vy och betalnings-vy---------------------------
        public static void CheckOut(List<ProductOrder> basket)
        {
            using var db = new MyDbContext();

            var customer = db.Customers.Where(x => x.Id == 1)
                .Select(x => x.Id)
                .FirstOrDefault();

            var allPaymentTypes = db.PaymentTypes.ToList();
            var allPayments = db.Payments.ToList();
            var allDeliveryTypes = db.DeliveryTypes.ToList();
            var allDeliveries = db.Deliveries.ToList();

            //---------------------------Betalnings-vy---------------------------

            Console.WriteLine("Available Payment Types");

            foreach (var allPaymentType in allPaymentTypes)
            {
                Console.WriteLine(allPaymentType.Id + " " + allPaymentType.PaymentTypeName);
            }

            Console.Write("Choose Payment Type (enter ID): ");

            var inputPaymentType = Helpers.GetGeneralId();

            var selectedPaymentType = allPaymentTypes.FirstOrDefault(x => x.Id == inputPaymentType);

            Console.WriteLine("Available Payments:");

            foreach (var allPayment in allPayments)
            {
                Console.WriteLine(allPayment.Id + " " + allPayment.PaymentName);
            }

            Console.Write("Choose Payment (enter ID): ");

            var inputPayment = Helpers.GetGeneralId();

            var selectedPayment = allPayments.FirstOrDefault(y => y.Id == inputPaymentType);

            //-------------------Frakt-vy---------------------------

            //Input av kundinforamtion
            Console.WriteLine("Please enter your information");
            var firstName = InputHelpers.GetInput("Enter First Name: ");
            var lastName = InputHelpers.GetInput("Enter Last Name: ");
            var address = InputHelpers.GetInput("Enter Address: ");
            var postalCode = InputHelpers.GetIntegerInput("Enter Postal Code: ");
            var city = InputHelpers.GetInput("Enter City: ");
            var country = InputHelpers.GetInput("Enter Country: ");
            var email = InputHelpers.GetInput("Enter email: ");
            var phoneNumber = InputHelpers.GetInput("Enter Phone Number: ");


            Console.WriteLine("Available Delivery Types:");
            foreach (var allDeliveryType in allDeliveryTypes)
            {
                Console.WriteLine(allDeliveryType.Id + " " + allDeliveryType.DeliveryName + " " + allDeliveryType.DeliveryPrice + ":-");
            }


            Console.Write("Choose Delivery Type (enter ID): ");

            var inputDeliveryType = Helpers.GetGeneralId();

            var selectedDeliveryType = allDeliveryTypes.FirstOrDefault(z => z.Id == inputDeliveryType);

            Console.WriteLine("Available Deliveries:");

            foreach (var allDelivery in allDeliveries)
            {
                Console.WriteLine(allDelivery.Id + " " + allDelivery.DeliveryName);
            }

            Console.Write("Choose Delivery (enter ID): ");

            var inputDelivery = Helpers.GetGeneralId();

            var selectedDelivery = allDeliveries.FirstOrDefault(c => c.Id == inputDelivery);

            //Beräkna totalt belopp
            var totalAmount = 0.0;

            totalAmount += selectedDeliveryType.DeliveryPrice;

            foreach (var product in basket)
            {
                totalAmount += product.ProductVariant.Product.Price.Value;
            }

            //---------------------------Visa sammanfattning---------------------------
            Console.WriteLine("Summary");
            ShowBasket(basket);
            Console.WriteLine($"Payment: {selectedPayment.PaymentName}");
            Console.WriteLine($"Payment_Type: {selectedPaymentType.PaymentTypeName}");
            Console.WriteLine($"Delivery: {selectedDelivery.DeliveryName}");
            Console.WriteLine($"Delivery_Type: {selectedDeliveryType.DeliveryName}");
            Console.WriteLine($"Delivery_Cost: {selectedDeliveryType.DeliveryPrice}:-");
            Console.WriteLine($"Total_Cost: {totalAmount}:-");

            //Totalpriset inklusive moms (25%)
            Console.WriteLine($"Total Cost with taxes included: {totalAmount * 0.25}:-");

            //Kunduppgifter
            var customerInfo = $"Your information: {firstName} {lastName}, {address}, {postalCode}, {city}, {country}, {country}, {phoneNumber}";

            var finishCheckOut = InputHelpers.GetYesOrNo("Wanna_finish?: ");
            if (finishCheckOut == true)
            {
                var productOrder = new FinalOrder()
                {
                    CustomerId = customer,
                    PaymentId = selectedPayment.Id,
                    DeliveryId = selectedDelivery.Id,
                    TotalPrice = totalAmount,
                };
                db.Add(productOrder);
                db.SaveChanges();

                //Töm varukorgen efter beställning
                basket.Clear();

                Console.WriteLine("Thank you for shopping :)");
            }
        }
        public static void ShowFeaturedProduct()
        {

            using var db = new MyDbContext();

            var products = db.Products.Where(x => x.FeaturedProduct == true).ToList();

            foreach (var product in products)
            {
                Console.WriteLine($"Product Name: {product.Name}");
                Console.WriteLine($"Description: {product.Description}");
                Console.WriteLine($"Price: {product.Price}");

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
                            Console.Write($"Size: {variant.Size.SizeName} - {variant.Colour.ColourName}");
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
                Console.WriteLine();
            }
        }
    }
}
