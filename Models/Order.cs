using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Modellklasser för att representera slutliga beställningar, leveranser, betalningar och relaterade entiteter
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
            Adresses = new HashSet<Adress>();

        }
        public int Id { get; set; }
        public int DeliveryNameId { get; set; }
        public int DeliveryTypeId { get; set; }
        public virtual ICollection<FinalOrder> FinalOrders { get; set; }
        public virtual ICollection<Adress> Adresses { get; set; }
        public virtual DeliveryType DeliveryType { get; set; }
        public virtual DeliveryName DeliveryName { get; set; }
    }
    public partial class DeliveryName
    {
        public DeliveryName()
        {
            Deliveries = new HashSet<Delivery>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Delivery> Deliveries { get; set; }
    }
    public partial class DeliveryType
    {
        public DeliveryType()
        {
            Deliveries = new HashSet<Delivery>();
        }
        public int Id { get; set; }
        public string DeliveryName { get; set; }
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
        public int PaymentNameId { get; set; }
        public int PaymentTypeId { get; set; }
        public virtual ICollection<FinalOrder> FinalOrders { get; set; }
        public virtual PaymentName PaymentName { get; set; }
        public virtual PaymentType PaymentType { get; set; }
    }
    public partial class PaymentName
    {
        public PaymentName()
        {
            Payments = new HashSet<Payment>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
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
