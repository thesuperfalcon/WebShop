﻿using Microsoft.EntityFrameworkCore;
using WebShop.Models;

namespace WebShop
{
    internal class LoginManager
    {
        public static Customer LoginMenu(MyDbContext dbContext)
        {
            var customer = new Customer();
            bool success = false;

            while (!success)
            {
                foreach (int i in Enum.GetValues(typeof(MyEnums.LoginMenu)))
                {
                    Console.WriteLine(i + ". " + Enum.GetName(typeof(MyEnums.LoginMenu), i).Replace('_', ' '));
                }
                int nr;
                if (int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out nr))
                {
                    MyEnums.LoginMenu menuSelection = (MyEnums.LoginMenu)nr;
                    switch (menuSelection)
                    {
                        case MyEnums.LoginMenu.Login:
                            customer = Login(dbContext, out success);
                            break;
                        case MyEnums.LoginMenu.CreateAccount:
                            customer = CreateCustomer();
                            success = true;
                            break;
                        case MyEnums.LoginMenu.Exit:
                            Environment.Exit(0);
                            break;
                    }

                }
                Thread.Sleep(500);
                Console.Clear();
            }

            return customer;
        }

        private static Customer Login(MyDbContext dbContext, out bool success)
        {
            var customer = new Customer();
            success = false;

            Console.Write("Enter your email: ");
            string enteredEmail = Console.ReadLine()?.ToLowerInvariant();
            Console.Write("Enter your password: ");
            string enteredPassword = Console.ReadLine()?.ToLowerInvariant();

            if (ValidateLogin(dbContext, enteredEmail, enteredPassword, out bool isAdmin, out string displayName, out Customer loggedInCustomer))
            {
                Console.WriteLine("Login successful!");

                customer = loggedInCustomer;

                if (isAdmin)
                {
                    Console.WriteLine($"Welcome to the admin page {displayName}");
                }
                else
                {
                    Console.WriteLine($"Welcome {displayName}");
                }

                success = true;
            }
            else
            {
                Console.WriteLine("Invalid email or password. Try again.");
            }

            return customer;
        }

        private static bool ValidateLogin(MyDbContext dbContext, string enteredEmail, string enteredPassword, out bool isAdmin, out string displayName, out Customer loggedInCustomer)
        {
            isAdmin = false;
            displayName = string.Empty;
            loggedInCustomer = null;

            if (string.IsNullOrWhiteSpace(enteredEmail) || string.IsNullOrWhiteSpace(enteredPassword))
            {
                return false;
            }

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

        private static Customer CreateCustomer()
        {
            using var db = new MyDbContext();

            string firstName = InputHelpers.GetInput("Enter first name: ");
            string lastName = InputHelpers.GetInput("Enter last name: ");

            var addressInfo = Helpers.CreateAddress(db);

            if (addressInfo == null)
            {
                return null;
            }

            Console.Write("Enter phone number (or press Enter to skip): ");
            string phoneNumberInput = Console.ReadLine();

            int? phoneNumber = !string.IsNullOrEmpty(phoneNumberInput) ? InputHelpers.GetIntegerInput(phoneNumberInput) : (int?)null;

            string email = InputHelpers.GetInput("Enter email: ");
            string password = InputHelpers.GetInput("Enter password: ");
            bool isAdmin = InputHelpers.GetYesOrNo("Is admin?");

            var existingFirstName = db.FirstName.FirstOrDefault(fn => fn.Name == firstName);

            if (existingFirstName == null)
            {
                existingFirstName = new FirstName { Name = firstName };
                db.FirstName.Add(existingFirstName);
                db.SaveChanges();
            }

            var existingLastName = db.LastName.FirstOrDefault(ln => ln.Name == lastName);

            if (existingLastName == null)
            {
                existingLastName = new LastName { Name = lastName };
                db.LastName.Add(existingLastName);
                db.SaveChanges();
            }

            var existingCustomer = db.Customers.FirstOrDefault(c => c.Email == email);

            if (existingCustomer != null)
            {
                bool hasAnotherEmail = InputHelpers.GetYesOrNo("A customer with the same email already exists. Do you have another email?: ");

                if (hasAnotherEmail)
                {
                    email = InputHelpers.GetInput("Enter another email: ");

                    if (db.Customers.Any(x => x.Email == email))
                    {
                        Console.WriteLine("A customer with the new email already exists. Returning to the menu.");
                        return null;
                    }

                    existingCustomer = null;
                }
                else
                {
                    Console.WriteLine("Returning to the menu.");
                    return null;
                }
            }

            var newCustomer = new Customer
            {
                FirstNameId = existingFirstName.Id,
                LastNameId = existingLastName.Id,
                AdressId = addressInfo.Id,
                PhoneNumber = phoneNumber,
                Email = email,
                Password = password,
                IsAdmin = isAdmin,
            };

            db.Customers.Add(newCustomer);
            db.SaveChanges();

            return newCustomer;
        }
    }
}