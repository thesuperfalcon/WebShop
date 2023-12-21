using Microsoft.Identity.Client;
using WebShop.Models;

namespace WebShop
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AddData.AddCustomerInfo();
            AddData.AddOrderInfo();
            //AddData.AddProductInfo();
            //TheMenu.ShowMenu();   
            //hejhej
            
        }
    }
}