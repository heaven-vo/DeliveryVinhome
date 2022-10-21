using System;
using System.Collections.Generic;

namespace DeliveryVHGP.Core.Entities
{
    public partial class OrderStatus
    {
        public OrderStatus()
        {
            Orders = new HashSet<Order>();
            TimeOfOrderStages = new HashSet<TimeOfOrderStage>();
        }

        public string Id { get; set; } = null!;
        public string? Name { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<TimeOfOrderStage> TimeOfOrderStages { get; set; }
    }
}
