using System;
using System.Collections.Generic;

namespace DeliveryVHGP.WebApi.Models
{
    public partial class Segment
    {
        public Segment()
        {
            SegmentDeliveries = new HashSet<SegmentDelivery>();
        }

        public string Id { get; set; } = null!;
        public string? BuildingId { get; set; }
        public string? HubId { get; set; }
        public string? OrderId { get; set; }
        public string? SegmentTaskId { get; set; }
        public string? SegmentMode { get; set; }
        public string? Status { get; set; }

        public virtual Building? Building { get; set; }
        public virtual Hub? Hub { get; set; }
        public virtual Order? Order { get; set; }
        public virtual SegmentTask? SegmentTask { get; set; }
        public virtual ICollection<SegmentDelivery> SegmentDeliveries { get; set; }
    }
}
