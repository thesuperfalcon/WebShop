using Microsoft.EntityFrameworkCore;
using System.Drawing;
using WebShop.Models;

namespace WebShop
{
    internal class TheMenu
    {
    //    static List<ProductOrder> basket = new List<ProductOrder>();

    //    public static void ShowMenu(Customer customer)
    //    {
    //        bool loop = true;
    //        while (loop)
    //        {
    //            using var db = new MyDbContext();

    //            var products = db.Products.Include(x => x.Categories).
    //                Include(y => y.Colours).
    //                Where(z => z.FeaturedProduct == true);

    //            foreach (var product in products)
    //            {
    //                Console.WriteLine(product.Name);
    //                Console.WriteLine(product.Price);
    //                foreach (var category in product.Categories)
    //                {
    //                    Console.Write(category.CategoryName + " ");
    //                }
    //                Console.WriteLine();
    //                foreach (var colour in product.Colours)
    //                {
    //                    Console.Write(colour.ColourName + " ");
    //                }
    //                Console.WriteLine();
    //            }

    //            Console.WriteLine();

    //            foreach (int i in Enum.GetValues(typeof(MyEnums.Menu)))
    //            {
    //                Console.WriteLine(i + ". " + Enum.GetName(typeof(MyEnums.Menu), i).Replace('_', ' '));
    //            }
    //            int nr;
    //            if (int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out nr))
    //            {
    //                MyEnums.Menu menuSelection = (MyEnums.Menu)nr;

    //                switch (menuSelection)
    //                {
    //                    case MyEnums.Menu.Search: SearchMenu(); break;
    //                    case MyEnums.Menu.Category: ShowCategories(); break;
    //                    case MyEnums.Menu.Cart: ShowBasket(); break;
    //                    case MyEnums.Menu.CheckOut: CheckOut(); break;
    //                    case MyEnums.Menu.Exit:
    //                        loop = false;
    //                        break;
    //                }
    //            }
    //            else
    //            {
    //                Console.WriteLine("Wrong input: ");
    //            }
    //            Console.ReadLine();
    //            Console.Clear();
    //        }
    //    }
    //    public static void SearchMenu()
    //    {
    //        using (var db = new MyDbContext())
    //        {
    //            Console.Clear();

    //            var productName = InputHelpers.GetInput("Search: ");

    //            var specificProduct = db.Products.Include(x => x.Categories)
    //                .Include(x => x.Colours)
    //                .Include(x => x.Sizes)
    //                .Where(x => productName.Contains(x.Name)).FirstOrDefault();

    //            if (specificProduct != null)
    //            {
    //                ShowProduct(specificProduct);
    //            }
    //        }
    //    }
    //    public static void ShowCategories()
    //    {
    //        using (var db = new MyDbContext())
    //        {
    //            Console.Clear();

    //            var categories = db.Categories.Where(x => x.Products.Count() > 0).ToList();

    //            foreach (var category in categories)
    //            {
    //                Console.WriteLine(category.CategoryName);
    //            }
    //            Console.Read();
    //        }
    //    }
    //    public static void ShowProduct(Product product)
    //    {
    //        using (var db = new MyDbContext())
    //        {
    //            Console.Clear();

    //            Console.WriteLine($"Name: {product.Name}");
    //            Console.WriteLine($"Price: {product.Price}");
    //            Console.Write("Categories: ");
    //            foreach (var category in product.Categories)
    //            {
    //                Console.Write($"{category.CategoryName} ");
    //            }
    //            Console.WriteLine();
    //            Console.Write("Colours: ");
    //            foreach (var colour in product.Colours)
    //            {
    //                Console.Write($"{colour.ColourName} ");
    //            }
    //            Console.WriteLine("Size: ");
    //            foreach (var size in product.Sizes)
    //            {
    //                Console.Write($"{size.SizeName} ");
    //            }
    //            Console.WriteLine($"Description: {product.Description}");
    //            Console.WriteLine($"Supplier: {product.ProductSupplier}");
    //            Console.WriteLine($"Amount left: {product.Amount}");

    //            bool addProduct = InputHelpers.GetYesOrNo("Wanna add to cart?: ");

    //            if (addProduct == true)
    //            {
    //                var size = InputHelpers.GetInput("Which size?: ").ToUpper();

    //                var specificSize = product.Sizes.Where(x => size.Contains(x.SizeName))
    //                    .Select(x => x.Id);

    //                if (product.Sizes.Any(x => size.Contains(x.SizeName)))
    //                {
    //                    string colour = string.Empty;
    //                    if (product.Colours.Count >= 2)
    //                    {
    //                        colour = InputHelpers.GetInput("Which colour?: ");
    //                    }
    //                    else
    //                    {
    //                        colour = product.Colours.Select(x => x.ColourName).FirstOrDefault().ToString();
    //                    }
    //                    if (product.Colours.Any(x => colour.Contains(x.ColourName)))
    //                    {
    //                        var quantity = InputHelpers.GetIntegerInput("How many?: ");

    //                        var productOrder = new ProductOrder()
    //                        {
    //                            ProductId = product.Id,
    //                            Quantity = quantity,
    //                            Price = (product.Price * quantity).Value,
    //                            SizeId = Helpers.GetSizeId(size, product),
    //                            ColourId = Helpers.GetColourId(colour, product),
    //                        };
    //                        db.Add(productOrder);
    //                        bool addCart = InputHelpers.GetYesOrNo("Finished?: ");
    //                        if (addCart == true)
    //                        {
    //                            basket.Add(productOrder);
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    public static void ShowBasket()
    //    {
    //        using (var db = new MyDbContext())
    //        {
    //            int number = 1;
    //            double totalPrice = 0;
    //            foreach (var item in basket)
    //            {
    //                Console.WriteLine(number + ".");

    //                var product = db.Products.Include(x => x.Categories)
    //                    .Include(x => x.Colours)
    //                    .Where(x => x.Id == item.ProductId).FirstOrDefault();

    //                var size = db.Sizes.Where(x => x.Id == item.SizeId)
    //                    .Select(x => x.SizeName).FirstOrDefault();

    //                var colour = db.Colours.Where(x => x.Id == item.ColourId)
    //                    .Select(x => x.ColourName).FirstOrDefault();

    //                Console.WriteLine($"Product: {product.Name}");
    //                Console.WriteLine($"Description: {product.Description}");
    //                Console.WriteLine($"Size: {size}");
    //                Console.WriteLine($"Colour: {colour}");
    //                Console.WriteLine($"Quantity: {item.Quantity}");
    //                Console.WriteLine($"Price: {item.Price}:-");
    //                totalPrice += item.Price;
    //                number++;
    //            }
    //            Console.WriteLine($"Total price: {totalPrice}:-");
    //            Console.WriteLine();
    //        }
    //    }
    //    public static void CheckOut()
    //    {
    //        using var db = new MyDbContext();

    //        var customer = db.Customers.Where(x => x.Id == 1)
    //            .Select(x => x.Id)
    //            .FirstOrDefault();

    //        var allPaymentTypes = db.PaymentTypes.ToList();
    //        var allPayments = db.Payments.ToList();
    //        var allDeliveryTypes = db.DeliveryTypes.ToList();
    //        var allDeliveries = db.Deliveries.ToList();

    //        foreach(var allPaymentType in allPaymentTypes)
    //        {
    //            Console.WriteLine(allPaymentType.Id + " " + allPaymentType.PaymentTypeName);
    //        }

    //        var inputPaymentType = Helpers.GetGeneralId();

    //        var selectedPaymentType = allPaymentTypes.FirstOrDefault(x => x.Id == inputPaymentType);

    //        foreach(var allPayment in allPayments)
    //        {
    //            Console.WriteLine(allPayment.Id + " " + allPayment.PaymentName);
    //        }
            
    //        var inputPayment = Helpers.GetGeneralId();

    //        var selectedPayment = allPayments.FirstOrDefault(y => y.Id == inputPaymentType);

    //        foreach(var allDeliveryType in allDeliveryTypes)
    //        {
    //            Console.WriteLine(allDeliveryType.Id + " " + allDeliveryType.DeliveryTypeName + " " + allDeliveryType.DeliveryPrice + ":-");
    //        }

    //        var inputDeliveryType = Helpers.GetGeneralId();

    //        var selectedDeliveryType = allDeliveryTypes.FirstOrDefault(z => z.Id == inputDeliveryType);

    //        foreach(var allDelivery in allDeliveries)
    //        {
    //            Console.WriteLine(allDelivery.Id + " " + allDelivery.DeliveryName);
    //        }

    //        var inputDelivery = Helpers.GetGeneralId();

    //        var selectedDelivery = allDeliveries.FirstOrDefault(c => c.Id == inputDelivery);

    //        var totalAmount = 0.0;

    //        totalAmount += selectedDeliveryType.DeliveryPrice;

    //        foreach(var product in basket)
    //        {
    //            totalAmount += product.Price;
    //        }

    //        Console.WriteLine("Summary");
    //        ShowBasket();
    //        Console.WriteLine($"Payment: {selectedPayment.PaymentName}");
    //        Console.WriteLine($"Payment_Type: {selectedPaymentType.PaymentTypeName}");
    //        Console.WriteLine($"Delivery: {selectedDelivery.DeliveryName}");
    //        Console.WriteLine($"Delivery_Type: {selectedDeliveryType.DeliveryTypeName}");
    //        Console.WriteLine($"Delivery_Cost: {selectedDeliveryType.DeliveryPrice}:-");
    //        Console.WriteLine($"Total_Cost: {totalAmount}:-");

    //        var finishCheckOut = InputHelpers.GetYesOrNo("Wanna_finish?: ");
    //        if(finishCheckOut == true)
    //        {
    //            var productOrder = new FinalOrder()
    //            {
    //                CustomerId = customer,
    //                PaymentId = selectedPayment.Id,
    //                DeliveryId = selectedDelivery.Id,
    //                TotalPrice = totalAmount,
    //            };
    //            db.Add(productOrder);
    //            db.SaveChanges();
    //            Console.WriteLine("Thank you for shopping :)");
    //        }
    //    }
    }
}
