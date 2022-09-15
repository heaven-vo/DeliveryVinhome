using System;
using System.Collections.Generic;

namespace DeliveryVHGP_WebApi.Models
{
    public partial class OrderStatus
    {
        public OrderStatus()
        {
            TimeOfOrderStages = new HashSet<TimeOfOrderStage>();
        }

        public string Id { get; set; } = null!;
        public string? Name { get; set; }

        public virtual ICollection<TimeOfOrderStage> TimeOfOrderStages { get; set; }
    }
}
