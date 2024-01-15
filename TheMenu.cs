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
                string customerFullName = $"{customer.FirstName?.Name} {customer.LastName?.Name}";

                Console.WriteLine($"User: {customerFullName}");

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
                            productBasket = Search.CategorySearch();
                            break;
                        case MyEnums.Menu.Cart:
                            ShowBasket(basket);
                            break;
                        case MyEnums.Menu.CheckOut:
                            CheckOut(basket, customer);
                            break;
                        case MyEnums.Menu.Exit:
                            RollbackQuantities(basket, db);
                            loop = false;
                            break;
                    }
                    basket.Add(productBasket);
                }
                else
                {
                    Console.WriteLine("Wrong input: ");
                }

                Console.ReadLine();
                Console.Clear();
            }
        }

        public static void ShowBasket(List<ProductOrder> basket)
        {
            using (var db = new MyDbContext())
            {
                int number = 1;
                double totalPrice = 0;

                if (basket.Count > 0)
                {
                    List<ProductOrder> validBasketItems = new List<ProductOrder>();

                    foreach (var item in basket)
                    {
                        var productVariant = GetProductVariant(db, item.ProductVariantId);

                        if (productVariant != null)
                        {
                            DisplayProductDetails(item, productVariant);

                            totalPrice += item.Quantity * (productVariant.Product.Price ?? 0.0);
                            validBasketItems.Add(item);
                        }

                        Console.WriteLine();
                        number++;
                    }

                    basket.Clear();
                    basket.AddRange(validBasketItems);

                    Console.WriteLine($"Total price: {totalPrice}:-");
                    Console.WriteLine();
                    var answer = InputHelpers.GetYesOrNo("Do you want to add more of a product or remove a product?: ");

                    if (answer == true)
                    {
                        Console.WriteLine("1. Change the quantity of a product.");
                        Console.WriteLine("2. Remove a product from the basket.");

                        int choice;
                        if (int.TryParse(Console.ReadLine(), out choice))
                        {
                            switch (choice)
                            {
                                case 1:
                                    UpdateQuantity(basket);
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
                else
                {
                    Console.WriteLine("No products in the basket.");
                }
            }
        }

        public static void RemoveProduct(List<ProductOrder> basket)
        {
            using (var db = new MyDbContext())
            {
                foreach (var item in basket)
                {
                    var productVariant = GetProductVariant(db, item.ProductVariantId);
                    Console.WriteLine($"{productVariant.Id} {productVariant.Product.Name}");
                }

                int productVariantId = InputHelpers.GetIntegerInput("Enter the ID of the product you want to remove: ");

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

        private static void DisplayProductDetails(ProductOrder item, ProductVariant productVariant)
        {
            Console.WriteLine($"Product: {productVariant.Product.Name}");
            Console.WriteLine($"Description: {productVariant.Product.Description}");
            Console.WriteLine($"Size: {productVariant.Size?.SizeName ?? "N/A"}");
            Console.WriteLine($"Colour: {productVariant.Colour?.ColourName ?? "N/A"}");
            Console.WriteLine($"Quantity: {item.Quantity}");
            Console.WriteLine($"Price: {productVariant.Product.Price ?? 0.0}:-");
        }

        public static ProductOrder AddProductToBasket(Product product)
        {
            using var db = new MyDbContext();

            try
            {
                var addProduct = InputHelpers.GetYesOrNo("Add to cart?");

                if (addProduct)
                {
                    var colourChoice = db.ProductVariants
                        .Where(x => x.ProductId == product.Id)
                        .Select(x => x.Colour)
                        .Distinct()
                        .ToList();

                    foreach (var colour in colourChoice)
                    {
                        Console.WriteLine($"{colour.Id} {colour.ColourName}");
                    }

                    var colourIdInput = InputHelpers.GetIntegerInput("ColourId");
                    var selectedColour = db.Colours.FirstOrDefault(x => x.Id == colourIdInput);

                    var sizeChoice = db.ProductVariants
                        .Where(x => x.ColourId == selectedColour.Id && x.ProductId == product.Id)
                        .Select(x => x.Size)
                        .Distinct()
                        .ToList();

                    foreach (var size in sizeChoice)
                    {
                        Console.WriteLine($"{size.Id} {size.SizeName}");
                    }

                    var sizeIdInput = InputHelpers.GetIntegerInput("SizeId");
                    var selectedSize = db.Sizes.FirstOrDefault(x => x.Id == sizeIdInput);

                    var productVariant = db.ProductVariants
                        .Where(x => x.ColourId == selectedColour.Id && x.ProductId == product.Id && x.SizeId == selectedSize.Id)
                        .FirstOrDefault();

                    if (productVariant == null)
                    {
                        Console.WriteLine("Product variant not found.");
                        return null;
                    }

                    Console.WriteLine($"Available Quantity: {productVariant.Quantity}");

                    var quantity = InputHelpers.GetIntegerInput("Amount?");

                    if (quantity >= 0 && quantity <= productVariant.Quantity)
                    {
                        double? productPrice = db.ProductVariants
                            .Where(pv => pv.Id == productVariant.Id)
                            .Select(pv => pv.Product.Price)
                            .FirstOrDefault();

                        double totalPrice = (productPrice * quantity).Value;

                        var addToBasket = InputHelpers.GetYesOrNo("Add to basket?");

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

        private static void DisplayBasket(List<ProductOrder> basket, MyDbContext db)
        {
            Console.WriteLine("Products in your basket:");

            foreach (var item in basket)
            {
                var productVariant = GetProductVariant(db, item.ProductVariantId);
                Console.WriteLine($"{productVariant.Id}. {productVariant.Product.Name} - Quantity: {item.Quantity}");
            }
        }

        private static ProductOrder GetChosenProduct(List<ProductOrder> basket, int productIdInput)
        {
            return basket.FirstOrDefault(p => p.ProductVariantId == productIdInput);
        }

        private static int GetProductIdInput()
        {
            int productIdInput;
            while (true)
            {
                Console.WriteLine("Enter the ID of the product you want to update (or 0 to cancel):");

                if (int.TryParse(Console.ReadLine(), out productIdInput))
                {
                    return productIdInput;
                }

                Console.WriteLine("Invalid input. Please enter a valid product ID.");
            }
        }

        private static void DisplayProductQuantityOptions(ProductOrder chosenProduct)
        {
            var chosenProductName = chosenProduct.ProductVariant?.Product?.Name;

            Console.WriteLine($"Current quantity of {chosenProductName}: {chosenProduct.Quantity}");
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. Add quantity");
            Console.WriteLine("2. Remove quantity");
        }

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

        private static void AddQuantity(ProductOrder chosenProduct)
        {
            using var db = new MyDbContext();
            Console.WriteLine("Enter the quantity you want to add:");

            if (int.TryParse(Console.ReadLine(), out int quantityToAdd))
            {
                chosenProduct.Quantity += quantityToAdd;
                var chosenProductName = chosenProduct.ProductVariant?.Product?.Name;
                Console.WriteLine($"Added {quantityToAdd} more of {chosenProductName} to your basket.");
                UpdateProductVariantQuantity(chosenProduct.ProductVariantId, quantityToAdd, db);
            }
            else
            {
                Console.WriteLine("Invalid quantity.");
            }
        }

        private static void RemoveQuantity(ProductOrder chosenProduct)
        {
            using var db = new MyDbContext();
            Console.WriteLine("Enter the quantity you want to remove:");

            if (int.TryParse(Console.ReadLine(), out int quantityToRemove))
            {
                if (quantityToRemove <= chosenProduct.Quantity)
                {
                    chosenProduct.Quantity -= quantityToRemove;
                    Console.WriteLine($"Removed {quantityToRemove} from {chosenProduct.ProductVariant.Product.Name} in your basket.");
                    UpdateProductVariantQuantity(chosenProduct.ProductVariantId, -quantityToRemove, db);
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

        private static void UpdateProductVariantQuantity(int productVariantId, int quantityChange, MyDbContext db)
        {
            var productVariant = db.ProductVariants.FirstOrDefault(pv => pv.Id == productVariantId);

            if (productVariant != null)
            {
                productVariant.Quantity += quantityChange;
                db.SaveChanges();
            }
        }
        private static void ShowFeaturedProduct()
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

                        Console.Write("Size: ");
                        var sizes = productVariants.Select(variant => variant.Size.SizeName).Distinct();
                        Console.WriteLine(string.Join(", ", sizes));

                        Console.Write("Colour: ");
                        var colors = productVariants.Select(variant => variant.Colour.ColourName).Distinct();
                        Console.WriteLine(string.Join(", ", colors));
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

        public static ProductVariant GetProductVariant(MyDbContext db, int productVariantId)
        {
            var productVariant = db.ProductVariants
                .Include(x => x.Product)
                .Include(x => x.Colour)
                .Include(x => x.Size)
                .FirstOrDefault(x => x.Id == productVariantId);

            return productVariant;
        }

        private static void RollbackQuantities(List<ProductOrder> basket, MyDbContext db)
        {
            foreach (var item in basket)
            {
                var productVariant = GetProductVariant(db, item.ProductVariantId);
                productVariant.Quantity += item.Quantity;
            }

            db.SaveChanges();
        }

        //---------------------------Frakt-vy och betalnings-vy---------------------------
        public static void CheckOut(List<ProductOrder> basket, Customer customer)
        {
            using var db = new MyDbContext();

            var allPaymentTypes = db.PaymentTypes.ToList();
            var allPayments = db.PaymentNames.ToList();
            var allDeliveryTypes = db.DeliveryTypes.ToList();
            var allDeliveries = db.DeliveryNames.ToList();

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
                Console.WriteLine(allPayment.Id + " " + allPayment.Name);
            }

            Console.Write("Choose Payment (enter ID): ");

            var inputPayment = Helpers.GetGeneralId();

            var selectedPaymentName = allPayments.FirstOrDefault(y => y.Id == inputPayment);

            // Get or create Payment entity
            var selectedPayment = GetOrCreatePayment(db, selectedPaymentName, selectedPaymentType);

            //-------------------Frakt-vy---------------------------

            var adress = Helpers.ShowAdressInformation(db, customer);

            var adressInfo = InputHelpers.GetYesOrNo($"Delivery to this adress?: ");
            if (adressInfo == true)
            {

            }
            else
            {
                adress = Helpers.CreateAddress(db);
            }

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
                Console.WriteLine(allDelivery.Id + " " + allDelivery.Name);
            }

            Console.Write("Choose Delivery (enter ID): ");

            var inputDelivery = Helpers.GetGeneralId();

            var selectedDeliveryName = allDeliveries.FirstOrDefault(c => c.Id == inputDelivery);

            var selectedDelivery = GetOrCreateDelivery(db, selectedDeliveryName, selectedDeliveryType, adress);

            var totalAmount = 0.0;

            totalAmount += selectedDeliveryType.DeliveryPrice;

            foreach (var productOrder in basket)
            {
                var productVariant = productOrder.ProductVariant;

                if (productVariant != null)
                {
                    Console.WriteLine($"Product: {productVariant.Product.Name}");
                    Console.WriteLine($"Description: {productVariant.Product.Description}");
                    Console.WriteLine($"Size: {productVariant.Size?.SizeName ?? "N/A"}");
                    Console.WriteLine($"Colour: {productVariant.Colour?.ColourName ?? "N/A"}");
                    Console.WriteLine($"Quantity: {productOrder.Quantity}");
                    Console.WriteLine($"Price: {productVariant.Product.Price ?? 0.0}:-");

                    totalAmount += productOrder.Quantity * (productVariant.Product.Price ?? 0.0);
                }
            }

            //---------------------------Visa sammanfattning---------------------------
            Console.WriteLine("Summary");
            ShowBasket(basket);
            Console.WriteLine($"Payment: {selectedPaymentName.Name}");
            Console.WriteLine($"Payment_Type: {selectedPaymentType.PaymentTypeName}");
            Console.WriteLine($"Delivery: {selectedDeliveryName.Name}");
            Console.WriteLine($"Delivery_Type: {selectedDeliveryType.DeliveryName}");
            Console.WriteLine($"Delivery_Cost: {selectedDeliveryType.DeliveryPrice}:-");
            Console.WriteLine($"Total_Cost: {totalAmount}:-");

            // Totalpriset inklusive moms (25%)

            var taxes = 1.25;

            var finalPrice = (totalAmount + selectedDeliveryType.DeliveryPrice) * taxes;

            Console.WriteLine($"Total Cost with taxes included: {finalPrice}:-");

            // Kunduppgifter

            var finishCheckOut = InputHelpers.GetYesOrNo("Wanna_finish?: ");
            if (finishCheckOut == true)
            {
                var productOrder = new FinalOrder()
                {
                    CustomerId = customer.Id,
                    PaymentId = selectedPayment.Id,
                    DeliveryId = selectedDelivery.Id,
                    TotalPrice = finalPrice,
                    ProductOrders = basket
                };
                db.Add(productOrder);
                db.SaveChanges();

                // Töm varukorgen efter beställning
                basket.Clear();

                Console.WriteLine("Thank you for shopping :)");
            }
        }

        private static Delivery GetOrCreateDelivery(MyDbContext db, DeliveryName deliveryName, DeliveryType deliveryType, Adress address)
        {
            var deliveryNameId = deliveryName.Id;
            var deliveryTypeId = deliveryType.Id;

            var existingDelivery = db.Deliveries
                .Where(x => x.DeliveryTypeId == deliveryTypeId && x.DeliveryNameId == deliveryNameId)
                .FirstOrDefault();

            if (existingDelivery != null)
            {
                existingDelivery.Adresses.Add(address);
                db.SaveChanges();
                return existingDelivery;
            }
            else
            {
                var newDelivery = new Delivery
                {
                    DeliveryNameId = deliveryNameId,
                    DeliveryTypeId = deliveryTypeId,
                };

                db.Deliveries.Add(newDelivery);
                db.SaveChanges();
                newDelivery.Adresses.Add(address);
                db.SaveChanges();


                return newDelivery;
            }
        }


        private static Payment GetOrCreatePayment(MyDbContext db, PaymentName paymentName, PaymentType paymentType)
        {
            var existingPayment = db.Payments
                .Include(p => p.PaymentName)
                .Include(p => p.PaymentType)
                .FirstOrDefault(p => p.PaymentName == paymentName && p.PaymentType == paymentType);

            if (existingPayment != null)
            {
                return existingPayment;
            }
            else
            {
                var newPayment = new Payment
                {
                    PaymentName = paymentName,
                    PaymentType = paymentType
                };

                db.Payments.Add(newPayment);
                db.SaveChanges();

                return newPayment;
            }
        }
    }
}