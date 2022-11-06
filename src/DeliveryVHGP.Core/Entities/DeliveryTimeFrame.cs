using System;
using System.Collections.Generic;

namespace DeliveryVHGP.Core.Entities
{
    public partial class DeliveryTimeFrame
    {
        public DeliveryTimeFrame()
        {
            Orders = new HashSet<Order>();
        }

        public string Id { get; set; } = null!;
        public string? MenuId { get; set; }
        public double? FromHour { get; set; }
        public double? ToHour { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public virtual Menu? Menu { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
