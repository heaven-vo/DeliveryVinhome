using System;
using System.Collections.Generic;

namespace DeliveryVHGP.Core.Entities
{
    public partial class StoreCategory
    {
        public StoreCategory()
        {
            Stores = new HashSet<Store>();
        }

        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public double? DefaultCommissionRate { get; set; }
        public string? Status { get; set; }

        public virtual ICollection<Store> Stores { get; set; }
    }
}
