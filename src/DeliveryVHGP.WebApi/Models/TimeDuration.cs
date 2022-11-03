using System;
using System.Collections.Generic;

namespace DeliveryVHGP.WebApi.Models
{
    public partial class TimeDuration
    {
        public TimeDuration()
        {
            Orders = new HashSet<Order>();
        }

        public string Id { get; set; } = null!;
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public bool? Status { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
