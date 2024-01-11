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

            var sizeId = product.ProductVariants
                        .Where(x => size == x.Size.SizeName)
                        .Select(x => x.Size.Id).FirstOrDefault();

            return sizeId;
        }
        public static int GetColourId(string colour, Product product)
        {
            using var db = new MyDbContext();

            var colourId = product.ProductVariants
                          .Where(x => colour.Contains(x.Colour.ColourName))
                          .Select(x => x.Id).FirstOrDefault();

            return colourId;
        }
        public static int GetGeneralId()
        {
            return InputHelpers.GetIntegerInput("Id: ");
        }
    }
}
