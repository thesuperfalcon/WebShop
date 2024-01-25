using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Modellklasser som representerar kundinformation, förnamn, efternamn, adress, stad och land.
namespace WebShop.Models
{
    public partial class Customer
    {
        public int Id { get; set; }
        public int FirstNameId { get; set; }
        public int LastNameId { get; set; }
        public int AdressId { get; set; }
        public int? PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }

        public virtual Adress? Adress { get; set; }
        public virtual FirstName? FirstName { get; set; }
        public virtual LastName? LastName { get; set; }
    }
    public partial class FirstName
    {
        public FirstName()
        {
            Customers = new HashSet<Customer>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Customer> Customers { get; set; }
    }
    public partial class LastName
    {
        public LastName()
        {
            Customers = new HashSet<Customer>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Customer> Customers { get; set; }
    }
    public partial class Adress
    {
        public Adress()
        {
            Customers = new HashSet<Customer>();
        }
        public int Id { get; set; }
        public string AdressName { get; set; }
        public int PostalCode { get; set; }
        public int CityId { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Delivery> Deliveries { get; set; }
        public virtual City City { get; set; }
    }
    public partial class City
    {
        public City()
        {
            Adresses = new HashSet<Adress>();
        }
        public int Id { get; set; }
        public string CityName { get; set; }
        public int CountryId { get; set; }
        public virtual ICollection<Adress> Adresses { get; set; }
        public virtual Country Country { get; set; }
    }
    public partial class Country
    {
        public Country()
        {
            Cities = new HashSet<City>();
        }
        public int Id { get; set; }
        public string CountryName { get; set; }
        public virtual ICollection<City> Cities { get; set; }
    }
}
