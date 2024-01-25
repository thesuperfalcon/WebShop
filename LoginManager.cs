using Microsoft.EntityFrameworkCore;
using WebShop.Models;

namespace WebShop
{
    // Hanterar inloggning och skapande av användarkonton
    internal class LoginManager
    {
        // Visar en meny för inloggning och skapande av nya konton.
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
                        case MyEnums.LoginMenu.Create_new_account:
                            customer = CreateCustomer(customer);
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

        // Metoden Login försöker logga in användaren genom att kontrollera angivet e-post och lösenord mot databasen.
        private static Customer Login(MyDbContext dbContext, out bool success)
        {
            var customer = new Customer();
            success = false;

            Console.Write("\nEnter your email: ");
            string enteredEmail = Console.ReadLine()?.ToLowerInvariant();
            Console.Write("Enter your password: ");
            string enteredPassword = Console.ReadLine()?.ToLowerInvariant();

            // Om inloggningen lyckas, visas meny för administratör eller vanlig kund och returnerar den inloggade kunden.
            if (ValidateLogin(dbContext, enteredEmail, enteredPassword, out bool isAdmin, out string displayName, out Customer loggedInCustomer))
            {
                Console.WriteLine("\nLogin successful!");
                Thread.Sleep(500);

                customer = loggedInCustomer;

                if (isAdmin)
                {
                    Console.Clear();
                    Console.WriteLine($"Welcome to the admin page {displayName}");
                    Thread.Sleep(500);
                }
                else
                {
                    Console.Clear();
                    TheMenu.ShowMenu(loggedInCustomer);
                }

                success = true;
            }
            else
            {
                Console.WriteLine("Invalid email or password. Try again.");
            }

            return customer;
        }

        // Verifierar giltigheten av inloggningsuppgifter i databasen.
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

        // Skapar en ny användare genom att samla in och validera användarinformation.
        public static Customer CreateCustomer(Customer customer)
        {
            using var db = new MyDbContext();
            Console.Clear();

            string firstName = InputHelpers.GetInput("Enter first name: ");
            string lastName = InputHelpers.GetInput("Enter last name: ");

            var addressInfo = Helpers.CreateAddress(db);

            if (addressInfo == null)
            {
                return null;
            }

            // Speciellt lösenord för bekräftelse av admin.
            var adminAccessPassword = "abc123";

            int PhoneNumber = InputHelpers.GetIntegerInput("Enter phone number: ");
            Console.Write("Enter email: ");
            string email = Console.ReadLine();
            Console.Write("Enter password: ");
            string password = Console.ReadLine();
            bool isAdmin = InputHelpers.GetYesOrNo("Is admin?: ");
            if (customer.IsAdmin == false)
            {
                if (isAdmin)
                {
                    Console.Write("Type password for admin-access: ");
                    var userInput = Console.ReadLine();
                    if (adminAccessPassword == userInput)
                    {
                        Console.WriteLine("Password valid! Admin-role added.");
                        isAdmin = true;
                    }
                    else
                    {
                        Console.WriteLine("Password invalid! Admin-role denided.");
                        isAdmin = false;
                    }
                }
            }
            else
            {

            }

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
                    Console.Clear();
                    return LoginMenu(db);
                }
            }

            var newCustomer = new Customer
            {
                FirstNameId = existingFirstName.Id,
                LastNameId = existingLastName.Id,
                AdressId = addressInfo.Id,
                PhoneNumber = PhoneNumber,
                Email = email,
                Password = password,
                IsAdmin = isAdmin,
            };

            db.Customers.Add(newCustomer);
            db.SaveChanges();

            if (isAdmin == true)
            {
                Console.WriteLine("Customer added, returning to admin menu.");
                Thread.Sleep(1500);
                Console.Clear();
            }
            else
            {
                Console.WriteLine("Customer added, returning to login screen.");
                Thread.Sleep(1500);
                Console.Clear();
                LoginMenu(db);
            }
            return newCustomer;
        }
    }
}