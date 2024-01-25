using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;
using WebShop.Models;
using static WebShop.WindowUI;

namespace WebShop
{
    // Hanterar huvudmenyn och kundkorgen
    internal class TheMenu
    {
        public static List<ProductOrder> basket = new List<ProductOrder>();


        public static void ShowMenu(Customer customer)
        {
            var welcomeWindow = new Window("", 0, 0, new List<string>());
            Console.ForegroundColor = ConsoleColor.Green;
            welcomeWindow.DrawMessage("Welcome to Tace!");
            Console.ResetColor();

            // Huvudloop för att visa menyn och hantera användarens val.
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

                // Hämta menyval från enum och skapa en meny.
                var menuOptions = Enum.GetValues(typeof(MyEnums.Menu))
                       .Cast<MyEnums.Menu>()
                       .Select(menu => Enum.GetName(typeof(MyEnums.Menu), menu).Replace('_', ' '))
                       .ToList();

                var windowMenu = new Window("Menu", 41, 4, menuOptions);
                WindowUI.Window.DrawWindow("Menu", 41, 4, menuOptions);

                // Visa utvalda produkter.
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
                    // Lägg till produkten i varukorgen
                    basket.Add(productBasket);
                    Console.ForegroundColor = ConsoleColor.Green;
                    welcomeWindow.DrawMessage("Welcome to Tace!");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine("Wrong input: ");
                }
                Console.SetCursorPosition(0, 27);
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                welcomeWindow.DrawMessage("Welcome to Tace!");
                Console.ResetColor();
            }
        }

        // CheckOut-metoden hanterar genomförandet av köpet baserat på kundens varukorg och val.
        public static void CheckOut(List<ProductOrder> basket, Customer customer)
        {
            Console.WriteLine("\nCHECKOUT");
            Console.WriteLine("----------------------------------");
            using var db = new MyDbContext();

            // Hämtar nödvändig information från databasen.
            var allPaymentTypes = db.PaymentTypes.ToList();
            var allPayments = db.PaymentNames.ToList();
            var allDeliveryTypes = db.DeliveryTypes.ToList();
            var allDeliveries = db.DeliveryNames.ToList();

            //Presenterar betalingstyper och betalningar och låter användaren göra val
            Console.WriteLine("Available Payment Types");

            foreach (var allPaymentType in allPaymentTypes)
            {
                Console.WriteLine(allPaymentType.Id + ". " + allPaymentType.PaymentTypeName);
            }

            Console.WriteLine("\nChoose Payment Type (enter ID) ");
            var inputPaymentType = Helpers.GetGeneralId();

            var selectedPaymentType = allPaymentTypes.FirstOrDefault(x => x.Id == inputPaymentType);

            Console.WriteLine("\nAvailable Payments:");

            foreach (var allPayment in allPayments)
            {
                Console.WriteLine(allPayment.Id + ". " + allPayment.Name);
            }

            Console.WriteLine("\nChoose Payment (enter ID) ");

            var inputPayment = Helpers.GetGeneralId();

            var selectedPaymentName = allPayments.FirstOrDefault(y => y.Id == inputPayment);

            var selectedPayment = Helpers.GetOrCreatePayment(db, selectedPaymentName, selectedPaymentType);

            // Visar befintlig adressinformation för kunden och låter användaren välja befintlig eller att skapa en ny adress

            Console.WriteLine("\n----Existing address----");
            var adress = Helpers.ShowAdressInformation(db, customer);
            var adressInfo = InputHelpers.GetYesOrNo($"Do you want your delivery to this adress?: ");

            if (adressInfo == true)
            {

            }
            else
            {
                adress = Helpers.CreateAddress(db);
            }

            //Presenterar leveranstyper och leveranser och låter användaren göra val
            Console.WriteLine("\nAvailable Delivery Types:");
            foreach (var allDeliveryType in allDeliveryTypes)
            {
                Console.WriteLine(allDeliveryType.Id + ". " + allDeliveryType.DeliveryName + ": " + allDeliveryType.DeliveryPrice + ":-");
            }

            Console.WriteLine("\nChoose Delivery Type (enter ID) ");

            var inputDeliveryType = Helpers.GetGeneralId();

            var selectedDeliveryType = allDeliveryTypes.FirstOrDefault(z => z.Id == inputDeliveryType);

            Console.WriteLine("\nAvailable Parcels:");

            foreach (var allDelivery in allDeliveries)
            {
                Console.WriteLine(allDelivery.Id + ". " + allDelivery.Name);
            }

            Console.WriteLine("\nChoose Parcel (enter ID) ");

            var inputDelivery = Helpers.GetGeneralId();

            var selectedDeliveryName = allDeliveries.FirstOrDefault(c => c.Id == inputDelivery);

            var selectedDelivery = Helpers.GetOrCreateDelivery(db, selectedDeliveryName, selectedDeliveryType, adress);

            var deliveryPrice = Math.Round(selectedDeliveryType.DeliveryPrice, 2);

            //Räknar ut totalpriset för varukorgen
            var basketTotalValue = Helpers.CalculateBasketValue(basket, db);

            // Totalpriset inklusive moms (25%)
            var taxes = 1.25;
            double totalPrice = Math.Round((basketTotalValue + deliveryPrice) * taxes, 2);

            // Visa sammanfattning för användaren
            Console.WriteLine("\n\tORDER SUMMARY");
            Console.WriteLine("----------------------------------");
            BasketHelpers.DisplayProductDetails(basket, db);
            Console.WriteLine("\n----------------------------------");
            Console.WriteLine($"Payment: {selectedPaymentName.Name}");
            Console.WriteLine($"Payment type: {selectedPaymentType.PaymentTypeName}");
            Console.WriteLine("\n----------------------------------");
            Console.WriteLine($"Delivery: {selectedDeliveryName.Name}");
            Console.WriteLine($"Delivery type: {selectedDeliveryType.DeliveryName}");
            Console.WriteLine($"Delivery cost: {selectedDeliveryType.DeliveryPrice}:-");
            Console.WriteLine("\n----------------------------------");
            Console.WriteLine($"Total cost including 25 % taxes: {totalPrice}:-");

            //Erbjuder användaren att slutföra köp
            var finishCheckOut = InputHelpers.GetYesOrNo("\nAre you done shopping?: ");
            if (finishCheckOut == true)
            {
                // Skapar ny beställning och sparar den i databasen.
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

                //Tömmer varukorgen efter genomfört köp
                basket.Clear();

                Console.WriteLine("\n\tThank you for your purchase! :)");
                Thread.Sleep(2000);
            }
        }
    }
}