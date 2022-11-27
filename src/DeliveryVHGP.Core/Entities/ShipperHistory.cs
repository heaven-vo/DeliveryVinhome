using System;
using System.Collections.Generic;

namespace DeliveryVHGP.Core.Entities
{
    public partial class ShipperHistory
    {
        public string Id { get; set; } = null!;
        public string? ShipperId { get; set; }
        public string? OrderId { get; set; }
        public int? ActionType { get; set; }
        public int? Status { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual Order? Order { get; set; }
        public virtual Shipper? Shipper { get; set; }
    }
}
