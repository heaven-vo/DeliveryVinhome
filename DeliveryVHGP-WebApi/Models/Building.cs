using System;
using System.Collections.Generic;

namespace DeliveryVHGP_WebApi.Models
{
    public partial class Building
    {
        public Building()
        {
            Addresses = new HashSet<Address>();
            Hubs = new HashSet<Hub>();
            Orders = new HashSet<Order>();
        }

        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public string? AreaId { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<Hub> Hubs { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
