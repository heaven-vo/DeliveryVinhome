using System;
using System.Collections.Generic;

namespace DeliveryVHGP.WebApi.Models
{
    public partial class Schedule
    {
        public Schedule()
        {
            DeliveryShiftOfShippers = new HashSet<DeliveryShiftOfShipper>();
        }

        public string Id { get; set; } = null!;
        public string? Day { get; set; }
        public string? Month { get; set; }
        public string? Year { get; set; }

        public virtual ICollection<DeliveryShiftOfShipper> DeliveryShiftOfShippers { get; set; }
    }
}
