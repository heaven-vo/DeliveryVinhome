using System;
using System.Collections.Generic;

namespace DeliveryVHGP.Core.Entities
{
    public partial class Service
    {
        public Service()
        {
            Orders = new HashSet<Order>();
        }

        public string Id { get; set; } = null!;
        public string? Name { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
