﻿using System;
using System.Collections.Generic;

namespace DeliveryVHGP.WebApi.Models
{
    public partial class SegmentDeliveryRoute
    {
        public SegmentDeliveryRoute()
        {
            RouteEdges = new HashSet<RouteEdge>();
        }

        public string Id { get; set; } = null!;
        public string? ShipperId { get; set; }
        public string? FromShipperId { get; set; }
        public string? ToShipperId { get; set; }
        public double? Distance { get; set; }
        public int? Duration { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }

        public virtual Shipper? Shipper { get; set; }
        public virtual ICollection<RouteEdge> RouteEdges { get; set; }
    }
}
