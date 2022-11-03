using System;
using System.Collections.Generic;

namespace DeliveryVHGP.WebApi.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderActionHistories = new HashSet<OrderActionHistory>();
            OrderDetails = new HashSet<OrderDetail>();
            Payments = new HashSet<Payment>();
            SegmentDeliveries = new HashSet<SegmentDelivery>();
            Segments = new HashSet<Segment>();
            TimeOfOrderStages = new HashSet<TimeOfOrderStage>();
        }

        public string Id { get; set; } = null!;
        public string? CustomerId { get; set; }
        public string? StoreId { get; set; }
        public string? MenuId { get; set; }
        public string? BuildingId { get; set; }
        public string? ServiceId { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Note { get; set; }
        public double? Total { get; set; }
        public double? ShipCost { get; set; }
        public string? DurationId { get; set; }
        public string? Status { get; set; }

        public virtual Building? Building { get; set; }
        public virtual Customer? Customer { get; set; }
        public virtual TimeDuration? Duration { get; set; }
        public virtual Menu? Menu { get; set; }
        public virtual Service? Service { get; set; }
        public virtual OrderStatus? StatusNavigation { get; set; }
        public virtual Store? Store { get; set; }
        public virtual ICollection<OrderActionHistory> OrderActionHistories { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<SegmentDelivery> SegmentDeliveries { get; set; }
        public virtual ICollection<Segment> Segments { get; set; }
        public virtual ICollection<TimeOfOrderStage> TimeOfOrderStages { get; set; }
    }
}
