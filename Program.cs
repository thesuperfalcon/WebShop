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
            AddData.AddProductInfo();
            //ddData.AddOrderInfo();
            //AddData.AddFirstProducts();
            //AddData.AddMultipleProducts();
            //AddData.AddNewCustomerWithInput();
            //AddData.AddFirstCustomers();
            AddData.AddMultipleProducts(); 

            Customer customer = new Customer();

            //Customer customer = LoginManager.Login(db);

            TheMenu.ShowMenu(customer);

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