using System;
using System.Collections.Generic;

namespace DeliveryVHGP.Core.Entities
{
    public partial class Shift
    {
        public Shift()
        {
            DeliveryShiftOfShippers = new HashSet<DeliveryShiftOfShipper>();
        }

        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }

        public virtual ICollection<DeliveryShiftOfShipper> DeliveryShiftOfShippers { get; set; }
    }
}
