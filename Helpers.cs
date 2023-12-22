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
    }
}
