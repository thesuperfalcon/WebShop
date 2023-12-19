using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Models
{
    public partial class Order
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int OrderAmount { get; set; }
        public int CustomerId { get; set; }
        public int PaymentId { get; set; }
        public int DeliveryId { get; set; }
        public double TotalPrice { get; set; }

        public virtual Product Product { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Payment Payment { get; set; }

    }
    public partial class Delivery
    {
        public Delivery() 
        {
            Orders = new HashSet<Order>();
        }   
        public int Id { get; set; } 
        public string DeliveryName { get; set; }
        public int DeliveryTypeId { get; set; }
        public virtual DeliveryType DeliveryType { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
    public partial class DeliveryType
    {
        public DeliveryType()
        {
            Deliveries = new HashSet<Delivery>();
        }
        public int Id { get; set; }
        public string DeliveryTypeName { get; set; }
        public double DeliveryPrice { get; set; }
        public virtual ICollection<Delivery> Deliveries { get; set; }
    }
    public partial class Payment
    {
        public Payment()
        {
            Orders = new HashSet<Order>();
        }
        public int Id { get; set; }
        public string PaymentName { get; set; }
        public int PaymentTypeId { get; set; }
        public PaymentType PaymentType { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
    public partial class PaymentType
    {
        public PaymentType()
        {
            Payments = new HashSet<Payment>();
        }
        public int Id { get; set; }
        public string PaymentTypeName { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
