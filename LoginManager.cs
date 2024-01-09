using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Models;

namespace WebShop
{
    internal class LoginManager
    {
        public static Customer Login(MyDbContext dbContext)
        {
            var customer = new Customer();
            bool success = false;
            while (!success)
            {
                Console.Write("Enter your email: ");
                string enteredEmail = Console.ReadLine().ToLower();
                Console.Write("Enter your password: ");
                string enteredPassword = Console.ReadLine().ToLower();

                if (ValidateLogin(enteredEmail, enteredPassword, out bool isAdmin, out string displayName, out Customer loggedInCustomer))
                {
                    Console.WriteLine("Login successful!");

                    customer = dbContext.Customers.Where(x => x.Email == enteredEmail && x.Password == enteredPassword).FirstOrDefault();

                    if (isAdmin)
                    {
                        Console.WriteLine($"Welcome to the admin page {displayName}");
                    }
                    else
                    {
                        Console.WriteLine($"Welcome {displayName}");
                        TheMenu.ShowMenu(customer);
                    }
                    success = true;
                }
                else
                {
                    Console.WriteLine("Invalid email or password. Try again.");
                }
                Console.ReadLine();
            }
            return customer;
        }

        private static bool ValidateLogin(string enteredEmail, string enteredPassword, out bool isAdmin, out string displayName, out Customer loggedInCustomer)
        {
            using var dbContext = new MyDbContext();
            isAdmin = false;
            displayName = string.Empty;
            loggedInCustomer = null;

            var customer = dbContext.Customers
                .Include(c => c.FirstName)
                .Include(c => c.LastName)
                .FirstOrDefault(c => c.Email == enteredEmail && c.Password == enteredPassword);

            if (customer != null)
            {
                isAdmin = customer.IsAdmin;
                displayName = $"{customer.FirstName?.Name} {customer.LastName?.Name}";
                loggedInCustomer = customer;
                return true;
            }
            return false;
        }
    }
}

