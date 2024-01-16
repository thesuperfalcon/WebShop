﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop
{
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
            AddProduct,
            RemoveProduct,
            ChangeProduct,
            ChangeFeatured,
            ShowInventoryBalance,
            OrderHistory,
            CustomerInformation,
            ShowStatistic,
            Exit
        }
        public enum LoginMenu
        {
            Login,
            CreateAccount,
            Exit = 9
        }
    }
}
