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
            //AddData.AddMultipleProducts(); ------ Måste fixas :) 

<<<<<<< HEAD
            //Customer customer = LoginManager.Login(db);

            TheMenu.ShowMenu();
=======
            //var product = db.Products.FirstOrDefault(x => x.Id == 1);

            Admin.AdminMenu();
            //Admin.AddProductVariants(product);
            //TheMenu.ShowMenu(customer);
>>>>>>> 1219f40583060905f67296e8b8da43146064fd28
            //TheMenu.ShowBasket();

            //Search.CategorySearch();
            //Search.SearchFunction();
        }
    }
}