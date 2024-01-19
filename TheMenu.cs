﻿using Microsoft.EntityFrameworkCore;
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

                Console.WriteLine($"User: {customerFullName}");

                BasketHelpers.ShowFeaturedProduct();
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

        //---------------------------Frakt-vy och betalnings-vy---------------------------
        private static void CheckOut(List<ProductOrder> basket, Customer customer)
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

            var selectedPayment = Helpers.GetOrCreatePayment(db, selectedPaymentName, selectedPaymentType);

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

            // Totalpriset inklusive moms (25%)

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

                // Töm varukorgen efter beställning
                basket.Clear();

                Console.WriteLine("Thank you for shopping :)");
            }
        }
    }
}