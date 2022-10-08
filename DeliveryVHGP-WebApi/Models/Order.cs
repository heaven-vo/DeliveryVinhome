using System;
using System.Collections.Generic;

namespace DeliveryVHGP_WebApi.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
            Payments = new HashSet<Payment>();
            TimeOfOrderStages = new HashSet<TimeOfOrderStage>();
        }

        public string Id { get; set; } = null!;
        public string? CustomerId { get; set; }
        public double? Total { get; set; }
        public string? Type { get; set; }
        public string? HubId { get; set; }
        public string? StoreId { get; set; }
        public string? MenuId { get; set; }
        public string? BuildingId { get; set; }
        public string? Note { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? StatusId { get; set; }
        public double? ShipCost { get; set; }
        public string? DurationId { get; set; }

        public virtual Building? Building { get; set; }
        public virtual Customer? Customer { get; set; }
        public virtual TimeDuration? Duration { get; set; }
        public virtual Hub? Hub { get; set; }
        public virtual Menu? Menu { get; set; }
        public virtual OrderStatus? Status { get; set; }
        public virtual Store? Store { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<TimeOfOrderStage> TimeOfOrderStages { get; set; }
    }
}
