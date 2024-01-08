using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
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

                var products = db.Products.Include(x => x.Categories).
                    Include(y => y.Colours).
                    Where(z => z.FeaturedProduct == true);

                foreach (var product in products)
                {
                    Console.WriteLine(product.Name);
                    Console.WriteLine(product.Price);
                    foreach (var category in product.Categories)
                    {
                        Console.Write(category.CategoryName + " ");
                    }
                    Console.WriteLine();
                    foreach (var colour in product.Colours)
                    {
                        Console.Write(colour.ColourName + " ");
                    }
                    Console.WriteLine();
                }

                Console.WriteLine();

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
                        case MyEnums.Menu.Search: SearchMenu(); break;
                        case MyEnums.Menu.Category: ShowCategories(); break;
                        case MyEnums.Menu.Cart: ShowBasket(); break;
                        case MyEnums.Menu.CheckOut: break;
                        case MyEnums.Menu.Exit:
                            loop = false;
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Wrong input: ");
                }
                Console.ReadLine();
                Console.Clear();
            }
        }
        public static void SearchMenu()
        {
            using (var db = new MyDbContext())
            {
                Console.Clear();

                var productName = InputHelpers.GetInput("Search: ");

                var specificProduct = db.Products.Include(x => x.Categories)
                    .Include(x => x.Colours)
                    .Include(x => x.Sizes)
                    .Where(x => productName.Contains(x.Name)).FirstOrDefault();

                if (specificProduct != null)
                {
                    ShowProduct(specificProduct);
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
        public static void ShowProduct(Product product)
        {
            using (var db = new MyDbContext())
            {
                Console.Clear();

                Console.WriteLine($"Name: {product.Name}");
                Console.WriteLine($"Price: {product.Price}");
                Console.Write("Categories: ");
                foreach (var category in product.Categories)
                {
                    Console.Write($"{category.CategoryName} ");
                }
                Console.WriteLine();
                Console.Write("Colours: ");
                foreach (var colour in product.Colours)
                {
                    Console.Write($"{colour.ColourName} ");
                }
                Console.WriteLine("Size: ");
                foreach (var size in product.Sizes)
                {
                    Console.Write($"{size.SizeName} ");
                }
                Console.WriteLine($"Description: {product.Description}");
                Console.WriteLine($"Supplier: {product.ProductSupplier}");
                Console.WriteLine($"Amount left: {product.Amount}");

                bool addProduct = InputHelpers.GetYesOrNo("Wanna add to cart?: ");

                if (addProduct == true)
                {
                    var size = InputHelpers.GetInput("Which size?: ");

                    var specificSize = product.Sizes.Where(x => size.Contains(x.SizeName))
                        .Select(x => x.Id);

                    if (product.Sizes.Any(x => size.Contains(x.SizeName)))
                    {

                        if(product.Colours.Count > 1)
                        {
                            var colour = InputHelpers.GetInput("Which colour?: ");

                            if (product.Colours.Any(x => colour.Contains(x.ColourName)))
                            {

                                var quantity = InputHelpers.GetIntegerInput("How many?: ");

                                var productOrder = new ProductOrder()
                                {
                                    ProductId = product.Id,
                                    Quantity = quantity,
                                    Price = (product.Price * quantity).Value,
                                    SizeId = Helpers.GetSizeId(size, product),
                                };
                                db.Add(productOrder);
                                bool addCart = InputHelpers.GetYesOrNo("Finished?: ");
                                if (addCart == true)
                                {
                                    basket.Add(productOrder);
                                    //db.SaveChanges();
                                }
                            }
                        }
                    }
                }
            }
        }
        public static void ShowBasket()
        {
            using (var db = new MyDbContext())
            {
                int number = 1;
                double totalPrice = 0;
                foreach(var item in basket)
                {
                    Console.WriteLine(number + ".");

                    var product = db.Products.Include(x => x.Categories)
                        .Include(x => x.Colours)
                        .Where(x => x.Id == item.ProductId).FirstOrDefault();

                    var size = db.Sizes.Where(x => x.Id == item.ProductId)
                        .Select(x => x.SizeName).FirstOrDefault();

                    Console.WriteLine($"Product: {product.Name}");
                    Console.WriteLine($"Description: {product.Description}");
                    Console.WriteLine($"Size: {size}");
                    Console.Write($"Colour: ");
                    foreach(var colour in product.Colours)
                    {
                        Console.Write(colour.ColourName + " ");
                    }
                    Console.WriteLine();
                    Console.WriteLine($"Quantity: {item.Quantity}");
                    Console.WriteLine($"Price: {item.Price}:-");
                    totalPrice += item.Price;
                    number++;
                }
                Console.WriteLine($"Total price: {totalPrice}:-");
                Console.WriteLine();
            }
        }
    }
}
