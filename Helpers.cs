using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebShop.Models;

namespace WebShop
{
    internal class Helpers
    {
        public static int GetSizeId(string size, Product product)
        {
            using var db = new MyDbContext();

            var sizeId = product.Sizes.Where(x => size.Contains(x.SizeName))
                .Select(x => x.Id).FirstOrDefault();

            return sizeId;
        }
        public static int GetColourId(string colour, Product product)
        {
            using var db = new MyDbContext();

            var colourId = product.Colours.Where(x => colour.Contains(x.ColourName))
                .Select(x => x.Id).FirstOrDefault();

            return colourId;
        }
        public static int GetGeneralId()
        {
            return InputHelpers.GetIntegerInput("Id: ");
        }
        public static string ValidateInput(string input)
        {
            while (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Invalid input. Please enter a non-empty value.");
                input = Console.ReadLine();
            }
            return input;
        }

        public static int ValidateIntInput(string input)
        {
            while (!int.TryParse(input, out int result))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                input = Console.ReadLine();
            }
            return int.Parse(input);
        }


        public static bool ValidateBoolInput(string input)
        {
            while (!bool.TryParse(input, out bool result))
            {
                Console.WriteLine("Invalid input. Please enter 'true' or 'false'.");
                input = Console.ReadLine();
            }
            return bool.Parse(input);
        }


    }
}
