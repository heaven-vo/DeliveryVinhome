using System;
using System.Collections.Generic;

namespace DeliveryVHGP.Core.Entities
{
    public partial class Order
    {
        public Order()
        {
            OrderActionHistories = new HashSet<OrderActionHistory>();
            OrderActions = new HashSet<OrderAction>();
            OrderDetails = new HashSet<OrderDetail>();
            Payments = new HashSet<Payment>();
            Segments = new HashSet<Segment>();
            ShipperHistories = new HashSet<ShipperHistory>();
            Transactions = new HashSet<Transaction>();
        }

        public string Id { get; set; } = null!;
        public string? StoreId { get; set; }
        public string? MenuId { get; set; }
        public string? BuildingId { get; set; }
        public string? ServiceId { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Note { get; set; }
        public double? Total { get; set; }
        public double? ShipCost { get; set; }
        public string? DeliveryTimeId { get; set; }
        public int? Status { get; set; }
        public string? MessageFail { get; set; }

        public virtual Building? Building { get; set; }
        public virtual DeliveryTimeFrame? DeliveryTime { get; set; }
        public virtual Menu? Menu { get; set; }
        public virtual Service? Service { get; set; }
        public virtual Store? Store { get; set; }
        public virtual OrderCache? OrderCache { get; set; }
        public virtual ICollection<OrderActionHistory> OrderActionHistories { get; set; }
        public virtual ICollection<OrderAction> OrderActions { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<Segment> Segments { get; set; }
        public virtual ICollection<ShipperHistory> ShipperHistories { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
