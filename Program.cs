using Microsoft.Identity.Client;
using WebShop.Models;

namespace WebShop
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var db = new MyDbContext();
            //AddData.AddCustomerInfo();
            //AddData.AddProductInfo();
            //AddData.AddOrderInfo();
            //AddData.AddFirstProducts();
            //AddData.AddMultipleProducts();
            //AddData.AddNewCustomerWithInput();
            //AddData.AddFirstCustomers();

            LoginManager loginManager = new LoginManager(db);

            Customer customer = loginManager.Login(db);

            TheMenu.ShowMenu(customer);

        }
    }
}