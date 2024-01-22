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

                //try
                //{
                //    AddData.RunAddDataMethods();
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine($"An error occurred: {ex.Message}");
                //}


                //Customer customer = new Customer();

                //////Customer customer = db.Customers.Where(x => x.Id == 2).SingleOrDefault();

                //customer = LoginManager.LoginMenu(db);

                ////AddData.RunAddDataMethods();

                Customer customer = LoginManager.LoginMenu(db);

                //Customer customer = db.Customers.FirstOrDefault(x => x.Id == 1);

                //Admin.AddProductVariants(product);


                //TheMenu.ShowMenu(customer);
                Admin.AdminMenu();

                //Admin.ChangeProduct();
                //Admin.ManageFeaturedProduct();

                //var product = db.Products.FirstOrDefault(x => x.Id == 1);

                //Admin.AddProductVariant
                ////Admin.AdminMenu();s(product);

            }
        }
    }
}
