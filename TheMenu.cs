using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using WebShop.Models;

namespace WebShop
{
    internal class TheMenu
    {
        public static void ShowMenu()
        {
            bool loop = true;
            while(loop)
            {
                using var db = new MyDbContext();

                var products = db.Products;

                foreach(int i in Enum.GetValues(typeof(MyEnums.Menu)))
                {
                    Console.WriteLine(i + ". " + Enum.GetName(typeof(MyEnums.Menu), i).Replace('_', ' '));
                }
                int nr;
                if(int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out nr))
                {
                    MyEnums.Menu menuSelection = (MyEnums.Menu)nr;
                    
                    switch(menuSelection)
                    {
                        case MyEnums.Menu.Search: break;
                        case MyEnums.Menu.Category: break;
                        case MyEnums.Menu.Cart: break;
                        case MyEnums.Menu.CheckOut: break;
                        case MyEnums.Menu.Exit:
                            loop = false;
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Wrong input: ");
                }
                Console.ReadLine(); 
                Console.Clear();
            }
        }
    }
}
