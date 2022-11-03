using System;
using System.Collections.Generic;

namespace DeliveryVHGP.WebApi.Models
{
    public partial class SegmentDelivery
    {
        public string Id { get; set; } = null!;
        public string? SegmentId { get; set; }
        public string? ShipperId { get; set; }
        public string? OrderId { get; set; }
        public string? FromShipperId { get; set; }
        public string? ToShipperId { get; set; }
        public double? Distance { get; set; }
        public int? Duration { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }

        public virtual Order? Order { get; set; }
        public virtual Segment? Segment { get; set; }
        public virtual Shipper? Shipper { get; set; }
    }
}
