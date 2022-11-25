using System;
using System.Collections.Generic;

namespace DeliveryVHGP.Core.Entities
{
    public partial class Shipper
    {
        public Shipper()
        {
            DeliveryShiftOfShippers = new HashSet<DeliveryShiftOfShipper>();
            SegmentDeliveryRoutes = new HashSet<SegmentDeliveryRoute>();
            SegmentTasks = new HashSet<SegmentTask>();
            ShipperHistories = new HashSet<ShipperHistory>();
        }

        public string Id { get; set; } = null!;
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Image { get; set; }
        public string? DeliveryTeam { get; set; }
        public bool? Status { get; set; }
        public string? CreateAt { get; set; }
        public string? UpdateAt { get; set; }
        public string? Email { get; set; }
        public string? VehicleType { get; set; }
        public string? LicensePlates { get; set; }
        public string? Colour { get; set; }

        public virtual ICollection<DeliveryShiftOfShipper> DeliveryShiftOfShippers { get; set; }
        public virtual ICollection<SegmentDeliveryRoute> SegmentDeliveryRoutes { get; set; }
        public virtual ICollection<SegmentTask> SegmentTasks { get; set; }
        public virtual ICollection<ShipperHistory> ShipperHistories { get; set; }
    }
}
