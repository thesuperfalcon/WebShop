using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Models
{
    public partial class ProductOrder
    {
        public int Id { get; set; }
        public int ProductVariantId { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
        public virtual ICollection<FinalOrder> FinalOrders { get; set; }
        public virtual ProductVariant ProductVariant { get; set; }
    }
}
