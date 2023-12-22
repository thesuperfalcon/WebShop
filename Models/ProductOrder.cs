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
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public int SizeId { get; set; }
        public virtual ICollection<FinalOrder> FinalOrders { get; set; }
        public virtual Product Product { get; set; }
    }
}
