using System;
using System.Collections.Generic;

namespace DeliveryVHGP_WebApi.Models
{
    public partial class Building
    {
        public Building()
        {
            Hubs = new HashSet<Hub>();
            Orders = new HashSet<Order>();
            Stores = new HashSet<Store>();
        }

        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public string? AreaId { get; set; }
        public string? CustomerId { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual ICollection<Hub> Hubs { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Store> Stores { get; set; }
    }
}
