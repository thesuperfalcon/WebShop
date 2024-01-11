﻿using Microsoft.EntityFrameworkCore;
using WebShop.Models;

namespace WebShop
{
    internal class TheMenu
    {
        static List<ProductOrder> basket = new List<ProductOrder>();

        public static void ShowMenu()
        {
            bool loop = true;
            while (loop)
            {
                using var db = new MyDbContext();

                var products = db.ProductVariants.Include(x => x.Product).
                    Include(y => y.Colour).
                    Where(z => z.Product.FeaturedProduct == true);

                foreach (var product in products)
                {
                    Console.WriteLine(product.Product.Name);
                    Console.WriteLine(product.Product.Price);
                    foreach (var category in product.Product.Categories)
                    {
                        Console.Write(category.CategoryName + " ");
                    }
                    Console.WriteLine();
                    foreach (var colour in product.Colour.ColourName)
                    {
                        Console.Write(colour + " ");
                    }
                    Console.WriteLine();
                }

                Console.WriteLine();

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
                        case MyEnums.Menu.Search: productBasket = Search.SearchFunction(); break;
                        case MyEnums.Menu.Category: ShowCategories(); break;
                        case MyEnums.Menu.Cart: ShowBasket(); break;
                        //case MyEnums.Menu.CheckOut: CheckOut(); break;
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

                foreach (var item in basket)
                {
                    var productName = db.ProductOrders.Include(x => x.ProductVariant).Where(x => x.ProductVariantId == item.ProductVariantId).FirstOrDefault();
                    Console.WriteLine(productName.ProductVariant.Product.Name);
                    //Console.WriteLine(item.ProductVariant.Colour.ColourName);
                    //Console.WriteLine(item.ProductVariant.Size.SizeName);
                    //Console.WriteLine(item.Quantity);
                }
                Console.ReadLine();
                Console.Clear();
            }
        }
        //public static void SearchMenu()
        //{
        //    using (var db = new MyDbContext())
        //    {
        //        Console.Clear();

        //        var productName = InputHelpers.GetInput("Search: ");

        //        var specificProduct = db.ProductVariants.Include(x => x.Product.Categories)
        //            .Include(x => x.Colour)
        //            .Include(x => x.Size)
        //            .Where(x => productName.Contains(x.Product.Name)).FirstOrDefault();

        //        if (specificProduct != null)
        //        {
        //            ShowProduct(specificProduct.Product);
        //        }
        //    }
        //}
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
        //public static void ShowProduct(Product product)
        //{
        //    using (var db = new MyDbContext())
        //    {
        //        Console.Clear();

        //        Console.WriteLine($"Name: {product.Name}");
        //        Console.WriteLine($"Price: {product.Price}");
        //        Console.Write("Categories: ");

        //        foreach (var category in product.Categories)
        //        {
        //            Console.Write($"{category.CategoryName} ");
        //        }
        //        Console.WriteLine();

        //        var productVariants = db.ProductVariants
        //                             .Include(x => x.Colour)
        //                             .Include(x => x.Size)
        //                             .Where(pv => pv.ProductId == product.Id)
        //                             .ToList();

        //        Console.Write("Colours: ");
        //        foreach (var color in productVariants)
        //        {
        //            Console.Write($"{color.Colour.ColourName} ");
        //        }
        //        Console.WriteLine();

        //        Console.WriteLine("Size: ");
        //        foreach (var size in productVariants)
        //        {
        //            Console.Write($"{size.Size.SizeName} ");
        //        }
        //        Console.WriteLine();

        //        Console.WriteLine($"Description: {product.Description}");
        //        Console.WriteLine($"Supplier: {product.ProductSupplier.SupplierName}");
        //        Console.WriteLine($"Amount left: {productVariants.Sum(p => p.Quantity)}");

        //        bool addProduct = InputHelpers.GetYesOrNo("Wanna add to cart?: ");

        //        if (addProduct == true)
        //        {
        //            var size = InputHelpers.GetInput("Which size?: ").ToUpper();

        //            var specificSize = productVariants.FirstOrDefault(x => size.Contains(x.Size.SizeName));

        //            if (specificSize != null)
        //            {
        //                string colour = string.Empty;

        //                if (productVariants.Count >= 2)
        //                {
        //                    colour = InputHelpers.GetInput("Which colour?: ");
        //                }
        //                else
        //                {
        //                    colour = productVariants.Select(x => x.Colour.ColourName).FirstOrDefault().ToString();
        //                }
        //                var specificColour = productVariants.FirstOrDefault(x => colour.Contains(x.Colour.ColourName));

        //                if (specificSize != null && specificColour != null)
        //                {
        //                    var quantity = InputHelpers.GetIntegerInput("How many?: ");



        //                    db.Add(productOrder);
        //                    bool addCart = InputHelpers.GetYesOrNo("Finished?: ");
        //                    if (addCart == true)
        //                    {
        //                        basket.Add(productOrder);
        //                    }

        //                }
        //            }
        //        }
        //    }
        //}

        public static ProductOrder AddProductToBasket(Product product)
        {
            using var Db = new MyDbContext();
            
            var addProduct = InputHelpers.GetYesOrNo("Add to cart?");
            if (addProduct == true)
            {

                var colourChoice = Db.ProductVariants.Where(x => x.ProductId == product.Id).Select(x => x.Colour).ToList();
                var colourChoiceEtt = colourChoice.Distinct().ToList();

                foreach (var colour in colourChoiceEtt)
                {
                    Console.WriteLine(colour.Id + " " + colour.ColourName);
                }
                var colourIdInput = InputHelpers.GetIntegerInput("ColourId");
                var selectedColour = Db.Colours.FirstOrDefault(x => x.Id == colourIdInput);

                var sizeChoice = Db.ProductVariants.Where(x => x.ColourId == selectedColour.Id && x.ProductId == product.Id).Select(x => x.Size).ToList();
                var sizeChoiceEtt = sizeChoice.Distinct().ToList();
                foreach (var size in sizeChoiceEtt)
                {
                    Console.WriteLine(size.Id + " " + size.SizeName);
                }
                var sizeIdInput = InputHelpers.GetIntegerInput("SizeId");
                var selectedSize = Db.Sizes.FirstOrDefault(x => x.Id == sizeIdInput);
                var quantity = InputHelpers.GetIntegerInput("Amount?");

                ProductVariant productOrder = Db.ProductVariants.Where(x => x.ColourId == selectedColour.Id && x.ProductId == product.Id && x.SizeId == selectedSize.Id).FirstOrDefault();

                var test = Db.ProductVariants.Where(x => x.Id == productOrder.Id).FirstOrDefault();
                Console.WriteLine(test.Id);
                Console.WriteLine(test.ProductId);
                Console.WriteLine(test.ColourId);
                Console.WriteLine(test.SizeId);
                Console.WriteLine(quantity);

                
                var addToBasket = InputHelpers.GetYesOrNo("Add to basket?");
                if (addToBasket == true)
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
            return null;
        }

        public static void ShowBasket()
        {
            var basket = new List<ProductOrder>();

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
            ShowBasket();

            using (var db = new MyDbContext())
            {
                Console.WriteLine("Enter the name of the product you want to add more of: ");
                string productName = Console.ReadLine();

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
            ShowBasket();

            using (var db = new MyDbContext())
            {
                Console.WriteLine("Enter the name of the product you want to remove: ");
                string productName = Console.ReadLine();

                var chosenProduct = basket.FirstOrDefault(p => p.ProductVariant.Product.Name == productName);
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
        public static void CheckOut()
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
            ShowBasket();
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
    }
}
