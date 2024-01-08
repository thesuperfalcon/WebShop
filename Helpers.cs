using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
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
    }
}
