using System;
using System.Collections.Generic;

namespace DeliveryVHGP.WebApi.Models
{
    public partial class SegmentTask
    {
        public SegmentTask()
        {
            Segments = new HashSet<Segment>();
        }

        public string Id { get; set; } = null!;
        public string? ShipperId { get; set; }
        public string? Status { get; set; }

        public virtual Shipper? Shipper { get; set; }
        public virtual ICollection<Segment> Segments { get; set; }
    }
}
