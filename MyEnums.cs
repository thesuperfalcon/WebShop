using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop
{
    // Enums som definierar menyalternativ för huvudmenyn, adminmenyn och inloggningsmenyn.
    internal class MyEnums
    {
        public enum Menu
        {
            Search,
            Category,
            Cart,
            CheckOut,
            Exit
        }
        public enum AdminMenu
        {
            Add_new_product,
            Remove_product,
            Change_product,
            Show_inventory_balance,
            Order_history,
            Customer_information,
            Add_new_customer,
            Show_statistic,
            Log_Out,
            Exit
        }
        public enum LoginMenu
        {
            Login,
            Create_new_account,
            Exit = 9
        }
    }
}
