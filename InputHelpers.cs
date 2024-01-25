﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop
{
    // En samling av hjälpmetoder för att interagera med användaren via konsolen,
    // hantera inmatningar och bekräftelser samt konvertera och formatera data.
    internal class InputHelpers
    {
        public static string GetInput(string prompt)
        {
            string input = string.Empty;
            while (string.IsNullOrWhiteSpace(input))
            {
                Console.Write(prompt);
                input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    TryAgain();
                }
            }
            return FormatString(input);
        }
        public static string FormatString(string input)
        {
            return char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }
        public static bool GetYesOrNo(string prompt)
        {
            while (true)
            {
                string input = GetInput(prompt).ToLower();
                if (input == "yes")
                {
                    return true;
                }
                else if (input == "no")
                {
                    return false;
                }
                else
                {
                    TryAgain();
                }
            }
        }
        public static int GetIntegerInput(string prompt)
        {
            int result;
            while (!int.TryParse(GetInput(prompt), out result))
            {
                TryAgain();
            }
            return result;
        }
        public static double GetDoubleInput(string prompt)
        {
            double result;
            while (!double.TryParse(GetInput(prompt), out result))
            {
                TryAgain();
            }
            return result;
        }
        private static void TryAgain()
        {
            Console.WriteLine("Try again.");
        }
    }
}
