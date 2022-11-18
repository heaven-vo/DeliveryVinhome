using System;
using System.Collections.Generic;

namespace DeliveryVHGP.WebApi.Models
{
    public partial class RouteEdge
    {
        public RouteEdge()
        {
            OrderActions = new HashSet<OrderAction>();
        }

        public string Id { get; set; } = null!;
        public string? FromBuildingId { get; set; }
        public string? ToBuildingId { get; set; }
        public int? Priority { get; set; }
        public double? Distance { get; set; }
        public string? RouteId { get; set; }
        public int? Status { get; set; }

        public virtual SegmentDeliveryRoute? Route { get; set; }
        public virtual ICollection<OrderAction> OrderActions { get; set; }
    }
}
