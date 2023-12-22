using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Models
{
    public partial class FinalOrder
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int PaymentId { get; set; }
        public int DeliveryId { get; set; }
        public double TotalPrice { get; set; }

        public virtual ICollection<ProductOrder> ProductOrders { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Payment Payment { get; set; }
        public virtual Delivery Delivery { get; set; }

    }
    public partial class Delivery
    {
        public Delivery() 
        {
            FinalOrders = new HashSet<FinalOrder>();
        }   
        public int Id { get; set; } 
        public string DeliveryName { get; set; }

        public virtual ICollection<DeliveryType> DeliveryTypes { get; set; }
        public virtual ICollection<FinalOrder> FinalOrders { get; set; }
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
            FinalOrders = new HashSet<FinalOrder>();
        }
        public int Id { get; set; }
        public string PaymentName { get; set; }
        public virtual ICollection<PaymentType> PaymentTypes { get; set; }
        public virtual ICollection<FinalOrder> FinalOrders { get; set; }
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
