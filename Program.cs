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
                //    Console.WriteLine("Done");
                //    Console.Read();
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine($"An error occurred: {ex.Message}");
                //}

                Customer customer = new Customer();
                //customer = LoginManager.LoginMenu(db);

                Customer customer1 = db.Customers.FirstOrDefault(customer => customer.Id == 2);

                if (customer1.IsAdmin == true)
                {
                    Admin.AdminMenu(customer1);
                }
                else
                {
                    TheMenu.ShowMenu(customer1);
                }

            }
        }
    }
}