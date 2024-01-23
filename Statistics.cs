using System;
using WebShop.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Dapper;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;

namespace WebShop
{
    public class Statistics
    {
        //Lagersaldo:Dapper
        public static void ShowInventoryBalance()
        {

            Console.WriteLine("---------------Inventory Balance---------------");

            string connString = "Data Source=DESKTOP-1ASCK61\\SQLEXPRESS;Initial Catalog=WebShopTestABC;Integrated Security=True;TrustServerCertificate=true;";
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();

                ShowStockCategory("Products with less than 1 in stock", "SELECT p.Name, SUM(DISTINCT pv.Quantity) AS TotalQuantity " +
                            "FROM Products p " +
                            "JOIN ProductVariants pv ON p.Id = pv.ProductId " +
                            "WHERE pv.Quantity <= 0 " +
                            "GROUP BY p.Name " +
                            "ORDER BY TotalQuantity DESC;", connection);

                ShowStockCategory("Products with between 1 and 50 in stock", "SELECT p.Name, COALESCE(SUM(pv.Quantity), 0) AS TotalQuantity " +
                    "FROM ProductVariants pv " +
                    "JOIN Products p ON p.Id = pv.ProductId " +
                    "GROUP BY p.Id, p.Name\n " +
                    "HAVING COALESCE(SUM(pv.Quantity), 0) BETWEEN 1 AND 50 " +
                    "ORDER BY TotalQuantity DESC;", connection);

                ShowStockCategory("Products with more than 50 in stock", "SELECT p.Name, SUM(DISTINCT pv.Quantity) AS TotalQuantity " +
                    "FROM Products p " +
                    "JOIN ProductVariants pv ON p.Id = pv.ProductId " +
                    "WHERE pv.Quantity > 50 " +
                    "GROUP BY p.Name " +
                    "ORDER BY TotalQuantity DESC;", connection);

                Console.WriteLine("-------------------------------------------");
                Console.Write("Press any key to return to the menu...");
                Console.ReadKey(true);
                return;
            }
        }

        public static void ShowStockCategory(string categoryTitle, string sqlQuery, SqlConnection connection)
        {
            using var db = new MyDbContext();

            Console.WriteLine($"\n\n{categoryTitle}");
            Console.WriteLine("-------------------------------------------");

            var stockProducts = connection.Query<StockInventory>(sqlQuery);

            if (stockProducts.Any())
            {
                int i = 0;
                foreach (var p in stockProducts)
                {
                    i++;
                    Console.WriteLine($"{i}. {p.Name}, Units left: {p.TotalQuantity} ");
                }
            }
            else
            {
                Console.WriteLine("No products found.");
            }
        }


        //Asynkron metod för orderhistorik i Dapper
        public static async Task OrderHistory()
        {
            Console.Clear();
            string connString = "Data Source=DESKTOP-1ASCK61\\SQLEXPRESS;Initial Catalog=WebShopTestABC;Integrated Security=True;TrustServerCertificate=true;";
            using (var connection = new SqlConnection(connString))
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                await connection.OpenAsync();

                var allOrders = await connection.QueryAsync<FinalOrder>("SELECT * FROM FinalOrders");

                Console.WriteLine("\n---------------Order history---------------");
                foreach (var finalOrders in allOrders)
                {
                    Console.WriteLine($"\nOrderID: {finalOrders.Id}, Customer: {finalOrders.CustomerId}, Payment: {finalOrders.PaymentId}, Delivery: {finalOrders.DeliveryId}, Total Price: {finalOrders.TotalPrice} ");
                }

                //Tidtagare för query
                stopwatch.Stop();
                Console.WriteLine($"\nQuery executed in: {stopwatch.ElapsedMilliseconds} milliseconds");


                Console.WriteLine("\n--------------------------------------------");

                Console.Write("Press any key to return to the menu...\n");
                Console.ReadKey(true);
                return;
            }
        }


        //Statestikmeny
        public static void ShowStatistic()
        {

            while (true)
            {

                Console.Clear();
                Console.WriteLine("-----Statstics Menu-----");
                Console.WriteLine("1. Best Selling Products");
                Console.WriteLine("2. Sales based on parcel services");
                Console.WriteLine("3. Sales based on payment method");
                Console.WriteLine("4. Most popular color");
                Console.WriteLine("5. Most popular sizes");
                Console.WriteLine("6. Return");
                Console.WriteLine("---------------------------------");
                int statisticChoice = InputHelpers.GetIntegerInput("Choose an option: ");

                switch (statisticChoice)
                {
                    case 1:
                        ShowBestSelling();
                        break;

                    case 2:
                        ShowPopularParcels();
                        break;

                    case 3:
                        ShowPopularPayment();
                        break;

                    case 4:
                        ShowTopColors();
                        break;

                    case 5:
                        ShowTopSizes();
                        break;

                    case 6:
                        Console.WriteLine("Returning to the main menu...");
                        return;

                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }


        //Populäraste produkterna: LINQ
        public static void ShowBestSelling()
        {
            Console.Clear();
            Console.WriteLine("\n---------------Our best selling products---------------");

            using (var dbContext = new MyDbContext())
            {
                var topSoldProducts = dbContext.ProductOrders
                    .Join(dbContext.ProductVariants, po => po.ProductVariantId, pv => pv.Id, (po, pv) => new { po, pv })
                    .Join(dbContext.Products, x => x.pv.ProductId, p => p.Id, (x, p) => new { x.po, p })
                    .GroupBy(x => x.p.Name)
                    .Select(g => new
                    {
                        ProductName = g.Key,
                        TotalSold = g.Sum(x => x.po.Quantity)
                    })
                    .OrderByDescending(x => x.TotalSold)
                    .Take(5)
                    .ToList();

                int i = 0;
                foreach (var product in topSoldProducts)
                {
                    i++;
                    Console.WriteLine($"\n{i}. {product.ProductName}, Total Sold: {product.TotalSold}");
                }
                Console.WriteLine("\n--------------------------------------------");

                Console.Write("Press any key to return to the menu...\n");

                Console.ReadKey(true);

                return;
            }
        }


        //Populäraste parcel: Dapper
        public static void ShowPopularParcels()
        {
            Console.Clear();
            Console.WriteLine("\n---------------Our most popular parcel service---------------");

            string connString = "Data Source=DESKTOP-1ASCK61\\SQLEXPRESS;Initial Catalog=WebShopTestABC;Integrated Security=True;TrustServerCertificate=true;";
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();

                var popularParcels = connection.Query<string>(
                    $"SELECT TOP (1) DN.Name as DeliveryName, COUNT(*) as Count " +
                     "FROM FinalOrders FO " +
                     "JOIN Deliveries D ON FO.DeliveryId = D.Id " +
                     "JOIN DeliveryNames DN ON D.DeliveryNameId = DN.Id " +
                     "GROUP BY DN.Name " +
                     "ORDER BY Count DESC");

                foreach (var parcelName in popularParcels)
                {
                    Console.WriteLine($"\nTop parcel: {parcelName}");
                }

                Console.WriteLine("\n--------------------------------------------");

                Console.Write("Press any key to return to the menu...\n");

                Console.ReadKey(true);

                return;
            }
        }


        //populäraste betalningsmetod: LINQ
        public static void ShowPopularPayment()
        {
            Console.Clear();
            Console.WriteLine("\n---------------Our most popular payment method---------------");

            using (var dbContext = new MyDbContext())
            {
                var paymentStatistics = dbContext.PaymentTypes
                .Join(dbContext.FinalOrders, paymentType => paymentType.Id, finalOrder => finalOrder.PaymentId, (paymentType, finalOrder) => new { PaymentType = paymentType, FinalOrder = finalOrder })
                .GroupBy(joinResult => joinResult.PaymentType.PaymentTypeName)
                .Select(group => new
                {
                    PaymentTypeName = group.Key,
                    OrderCount = group.Count()
                })
                 .OrderByDescending(result => result.OrderCount)
                 .ToList();

                foreach (var result in paymentStatistics)
                {
                    Console.WriteLine($"\nTop payment type: {result.PaymentTypeName}, Order count: {result.OrderCount}");
                }
                Console.WriteLine("\n--------------------------------------------");

                Console.Write("Press any key to return to the menu...\n");

                Console.ReadKey(true);

                return;
            }
        }

        //populäraste färgerna: LINQ
        public static void ShowTopColors()
        {
            Console.Clear();
            Console.WriteLine("\n---------------Our most popular colors---------------");

            using (var dbContext = new MyDbContext())
            {
                var mostPopularColours = dbContext.ProductOrders
               .Join(dbContext.ProductVariants, po => po.ProductVariantId, pv => pv.Id, (po, pv) => new { po, pv })
               .Join(dbContext.Colours, x => x.pv.ColourId, c => c.Id, (x, c) => new { x.po, c })
               .GroupBy(x => x.c.ColourName)
               .OrderByDescending(g => g.Count())
               .Select(g => g.Key)
               .Take(3)
               .ToList();

                int i = 0;
                foreach (var colour in mostPopularColours)
                {
                    i++;
                    Console.WriteLine($"\n{i}. {colour}");
                }
            }
            Console.WriteLine("\n--------------------------------------------");

            Console.Write("Press any key to return to the menu...\n");

            Console.ReadKey(true);

            return;
        }


        //populäraste storlekarna: Dapper
        public static void ShowTopSizes()
        {
            Console.Clear();
            Console.WriteLine("\n---------------Our top sizes---------------");

            string connString = "Data Source=DESKTOP-1ASCK61\\SQLEXPRESS;Initial Catalog=WebShopTestABC;Integrated Security=True;TrustServerCertificate=true;";
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();

                var popularSizes = connection.Query<string>(
                    $"SELECT TOP (3) S.SizeName, COUNT(*) as Count " +
                     "FROM ProductOrders PO " +
                     "JOIN ProductVariants PV ON PO.ProductVariantId = PV.Id " +
                     "JOIN Sizes S ON PV.SizeId = S.Id " +
                     "GROUP BY S.SizeName " +
                     "ORDER BY Count DESC");

                int i = 0;
                foreach (var sizeName in popularSizes)
                {
                    i++;
                    Console.WriteLine($"\n{i}. Size: {sizeName}");
                }
                Console.WriteLine("\n--------------------------------------------");

                Console.Write("Press any key to return to the menu...\n");

                Console.ReadKey(true);

                return;
            }
        }
    }
}