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


<<<<<<< Updated upstream
<<<<<<< HEAD
            Customer customer = new Customer();
=======
                //Customer customer = new Customer();
>>>>>>> c85c68c4c47a8d7448c838053c21ec79ef71d2ad
=======
                Customer customer = new Customer();
>>>>>>> Stashed changes

                //////Customer customer = db.Customers.Where(x => x.Id == 2).SingleOrDefault();

                customer = LoginManager.LoginMenu(db);

                ////AddData.RunAddDataMethods();


                //Customer customer = LoginManager.LoginMenu(db);

                //Customer customer = db.Customers.FirstOrDefault(x => x.Id == 1);

<<<<<<< Updated upstream
<<<<<<< HEAD
            //TheMenu.ShowMenu(customer);
            //Admin.AdminMenu();
            //Admin.ChangeProduct();
            //Admin.ManageFeaturedProduct();

            //var product = db.Products.FirstOrDefault(x => x.Id == 1);

            //Admin.AdminMenu();
            //Admin.AddProductVariants(product);

            TheMenu.ShowMenu(customer);


=======
                //TheMenu.ShowMenu(customer);
                Admin.AdminMenu();
=======
                TheMenu.ShowMenu(customer);
                //Admin.AdminMenu();
>>>>>>> Stashed changes
                //Admin.ChangeProduct();
                //Admin.ManageFeaturedProduct();


                //var product = db.Products.FirstOrDefault(x => x.Id == 1);
                //TheMenu.ShowMenu(customer);

                //Admin.AddProductVariant
                ////Admin.AdminMenu();s(product);
                //TheMenu.ShowMenu(customer);
            }
>>>>>>> c85c68c4c47a8d7448c838053c21ec79ef71d2ad
        }
    }
}
