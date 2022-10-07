using System;
using System.Collections.Generic;

namespace DeliveryVHGP_WebApi.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Buildings = new HashSet<Building>();
            Orders = new HashSet<Order>();
        }

        public string Id { get; set; } = null!;
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Image { get; set; }

        public virtual ICollection<Building> Buildings { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
