using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;
using WebShop.Models;
using static WebShop.WindowUI;

namespace WebShop
{
    internal class TheMenu
    {
        public static List<ProductOrder> basket = new List<ProductOrder>();
        public static void ShowMenu(Customer customer)
        {
            var welcomeWindow = new Window("", 0, 0, new List<string>());
            Console.ForegroundColor = ConsoleColor.Green;
            welcomeWindow.DrawMessage("Welcome to Tace!");
            Console.ResetColor();

            bool loop = true;
            while (loop)
            {
                using var db = new MyDbContext();
                string customerFullName = $"{customer.FirstName?.Name} {customer.LastName?.Name}";

                var updatedBasket = new List<ProductOrder>();

                foreach (var item in basket)
                {
                    if (item != null && item.Quantity > 0)
                    {
                        updatedBasket.Add(item);
                    }
                }
                int x = 0;

                basket = updatedBasket;

                var messageWindow = new Window("", 20, 0, new List<string>());
                messageWindow.DrawMessage($"User: {customerFullName}");

                var menuOptions = Enum.GetValues(typeof(MyEnums.Menu))
                       .Cast<MyEnums.Menu>()
                       .Select(menu => Enum.GetName(typeof(MyEnums.Menu), menu).Replace('_', ' '))
                       .ToList();

                var windowMenu = new Window("Menu", 41, 4, menuOptions);
                WindowUI.Window.DrawWindow("Menu", 41, 4, menuOptions);

                BasketHelpers.ShowFeaturedProduct();
                var productBasket = new ProductOrder();

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
                            if (basket != null)
                            {
                                BasketHelpers.ShowBasket(basket);
                            }
                            break;
                        case MyEnums.Menu.CheckOut:
                            CheckOut(basket, customer);
                            break;
                        case MyEnums.Menu.Exit:
                            BasketHelpers.RollbackQuantities(basket, db);
                            loop = false;
                            Environment.Exit(0);
                            break;
                    }
                    basket.Add(productBasket);
                    Console.ForegroundColor = ConsoleColor.Green;
                    welcomeWindow.DrawMessage("Welcome to Tace!");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine("Wrong input: ");
                }
                Console.ReadLine();
                Console.Clear();
            }
        }

        //---------------------------Frakt-vy och betalnings-vy---------------------------
        public static async Task CheckOut(List<ProductOrder> basket, Customer customer)
        {
            using var db = new MyDbContext();

            var allPaymentTypes = db.PaymentTypes.ToList();
            var allPayments = db.PaymentNames.ToList();
            var allDeliveryTypes = db.DeliveryTypes.ToList();
            var allDeliveries = db.DeliveryNames.ToList();

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

            var selectedPayment = Helpers.GetOrCreatePayment(db, selectedPaymentName, selectedPaymentType);

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

            var selectedDelivery = Helpers.GetOrCreateDelivery(db, selectedDeliveryName, selectedDeliveryType, adress);

            var deliveryPrice = Math.Round(selectedDeliveryType.DeliveryPrice, 2);

            // Calculate the total value of the basket
            var basketTotalValue = Helpers.CalculateBasketValue(basket, db);

            // Totalpriset inklusive moms (25%)

            var taxes = 1.25;

            double totalPrice = Math.Round((basketTotalValue + deliveryPrice) * taxes, 2);

            //---------------------------Visa sammanfattning---------------------------
            Console.WriteLine("Summary");
            BasketHelpers.DisplayProductDetails(basket, db);
            Console.WriteLine($"Payment: {selectedPaymentName.Name}");
            Console.WriteLine($"Payment_Type: {selectedPaymentType.PaymentTypeName}");
            Console.WriteLine($"Delivery: {selectedDeliveryName.Name}");
            Console.WriteLine($"Delivery_Type: {selectedDeliveryType.DeliveryName}");
            Console.WriteLine($"Delivery_Cost: {selectedDeliveryType.DeliveryPrice}:-");
            Console.WriteLine($"Total_Cost including 25 % taxes: {totalPrice}:-");

            var finishCheckOut = InputHelpers.GetYesOrNo("Wanna_finish?: ");
            if (finishCheckOut == true)
            {
                var productOrder = new FinalOrder()
                {
                    CustomerId = customer.Id,
                    PaymentId = selectedPayment.Id,
                    DeliveryId = selectedDelivery.Id,
                    TotalPrice = totalPrice,
                    ProductOrders = basket
                };
                db.Add(productOrder);
                db.SaveChanges();

                basket.Clear();

                Console.WriteLine("Thank you for shopping :)");
            }
        }
    }
}