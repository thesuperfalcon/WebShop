using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Models;

namespace WebShop
{

    public class LoginManager
    {
        public MyDbContext dbContext;

        public LoginManager(MyDbContext context)
        {
            dbContext = context;
        }

        public Customer Login(MyDbContext dbContext)
        {
            var customer = new Customer();
            while (true)
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
                        TheMenu.ShowMenu( customer );
                       
                    }
                }
                else
                {
                    Console.WriteLine("Invalid email or password. Try again.");
                }

                Console.ReadLine();
            }
            return customer;
            
            
        }

        private bool ValidateLogin(string enteredEmail, string enteredPassword, out bool isAdmin, out string displayName, out Customer loggedInCustomer)
        {
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
