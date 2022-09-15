using System;
using System.Collections.Generic;

namespace DeliveryVHGP_WebApi.Models
{
    public partial class Shipper
    {
        public Shipper()
        {
            DeliveryShiftOfShippers = new HashSet<DeliveryShiftOfShipper>();
        }

        public string Id { get; set; } = null!;
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Sex { get; set; }
        public string? Address { get; set; }
        public string? Age { get; set; }
        public string? Image { get; set; }

        public virtual ICollection<DeliveryShiftOfShipper> DeliveryShiftOfShippers { get; set; }
    }
}
