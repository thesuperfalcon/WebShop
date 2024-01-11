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

            //Customer customer = LoginManager.Login(db);
<<<<<<< HEAD
=======

>>>>>>> 65d55e73ce4221aad45f953d178cfb68948430aa
            //TheMenu.ShowMenu(customer);
            TheMenu.ShowBasket();

            Search.CategorySearch();
            //Search.SearchFunction();
        }
    }
}