using Microsoft.Identity.Client;
using WebShop.Models;

namespace WebShop
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var db = new MyDbContext();
            //AddData.AddProductInfo();
            //AddData.AddMultipleProducts();


            //AddData.AddCustomerInfo();

            ////ddData.AddOrderInfo();

            //AddData.AddNewCustomerWithInput();
            //AddData.AddFirstCustomers();

            //AddData.RunAddDataMethods();


            //Customer customer = LoginManager.LoginMenu(db);

            //Customer customer = db.Customers.FirstOrDefault(x => x.Id == 1);

            //TheMenu.ShowMenu(customer);
            Admin.AdminMenu();
            Admin.ChangeProduct();
            Admin.ManageFeaturedProduct();
            

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