using System;
using System.Collections.Generic;

namespace DeliveryVHGP_WebApi.Models
{
    public partial class Hub
    {
        public Hub()
        {
            DeliveryShiftOfShippers = new HashSet<DeliveryShiftOfShipper>();
            Orders = new HashSet<Order>();
        }

        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public string? BuildId { get; set; }
        public string? Room { get; set; }

        public virtual Building? Build { get; set; }
        public virtual ICollection<DeliveryShiftOfShipper> DeliveryShiftOfShippers { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
