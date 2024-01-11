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


            //Customer customer = LoginManager.Login(db);

            TheMenu.ShowMenu();

            //var product = db.Products.FirstOrDefault(x => x.Id == 1);

            //Admin.AdminMenu();
            //Admin.AddProductVariants(product);
            //TheMenu.ShowMenu(customer);

            //TheMenu.ShowBasket();

            //Search.CategorySearch();
            //Search.SearchFunction();
        }
    }
}