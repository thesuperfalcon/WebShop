using Microsoft.Identity.Client;
using WebShop.Models;
using static WebShop.WindowUI;

namespace WebShop
{
    internal class Program
    {
        //Huvudprogrammet
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
                customer = db.Customers.FirstOrDefault(x => x.Id == 1);

                if (customer.IsAdmin == true)
                {
                    Admin.AdminMenu(customer);
                }
                else
                {
                    TheMenu.ShowMenu(customer);
                }
            }
        }
    }
}