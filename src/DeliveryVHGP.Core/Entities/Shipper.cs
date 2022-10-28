using System;
using System.Collections.Generic;

namespace DeliveryVHGP.Core.Entities
{
    public partial class Shipper
    {
        public Shipper()
        {
            DeliveryShiftOfShippers = new HashSet<DeliveryShiftOfShipper>();
            SegmentDeliveries = new HashSet<SegmentDelivery>();
            SegmentTasks = new HashSet<SegmentTask>();
        }

        public string Id { get; set; } = null!;
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? VehicleType { get; set; }
        public string? Image { get; set; }
        public string? DeliveryTeam { get; set; }
        public string? Status { get; set; }
        public string? CreateAt { get; set; }
        public string? UpdateAt { get; set; }
        public string? Email { get; set; }

        public virtual ICollection<DeliveryShiftOfShipper> DeliveryShiftOfShippers { get; set; }
        public virtual ICollection<SegmentDelivery> SegmentDeliveries { get; set; }
        public virtual ICollection<SegmentTask> SegmentTasks { get; set; }
    }
}
