using Microsoft.Identity.Client;
using WebShop.Models;
using static WebShop.WindowUI;

namespace WebShop
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var db = new MyDbContext();

            while (true)
            {
                Customer customer = new Customer();
                customer = LoginManager.LoginMenu(db);

                if (customer.IsAdmin == true)
                {
                    Admin.AdminMenu(customer);
                }
                else
                {
                    TheMenu.ShowMenu(customer);
                }


                Customer customer = new Customer();
                customer = LoginManager.LoginMenu(db);

<<<<<<< HEAD
=======
                if (customer.IsAdmin == true)
                {
                    Admin.AdminMenu(customer);
                }
                else
                {
                    TheMenu.ShowMenu(customer);
                }


                //try
                //{
                //    AddData.RunAddDataMethods();
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine($"An error occurred: {ex.Message}");
                //}

>>>>>>> 064da91cf9821d65dbe42895bc57a403ae0be540
                //////AddData.RunAddDataMethods();
                //Customer customer = LoginManager.LoginMenu(db);
                //Customer customer = db.Customers.FirstOrDefault(x => x.Id == 1);
                //Customer customer = customer = LoginManager.LoginMenu(db);
<<<<<<< HEAD
                ////////Customer customer = db.Customers.Where(x => x.Id == 2).SingleOrDefault();
=======
                //Customer customer = db.Customers.Where(x => x.Id == 2).SingleOrDefault();
>>>>>>> 064da91cf9821d65dbe42895bc57a403ae0be540

                //Admin.AddProductVariants(product);
                //Admin.ChangeProduct();
                //Admin.ManageFeaturedProduct();
<<<<<<< HEAD
                //var product = db.Products.FirstOrDefault(x => x.Id == 1);
                //var product = db.Products.FirstOrDefault(x => x.Id == 1);
                //Admin.AdminMenu(customer);
=======
                //var product = db.Products.FirstOrDefault(x => x.Id == 1);
                //var product = db.Products.FirstOrDefault(x => x.Id == 1);
                //Admin.AdminMenu(customer);

>>>>>>> 064da91cf9821d65dbe42895bc57a403ae0be540
            }
        }
    }
}