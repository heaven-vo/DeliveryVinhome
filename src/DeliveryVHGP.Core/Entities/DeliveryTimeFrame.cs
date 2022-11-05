using System;
using System.Collections.Generic;

namespace DeliveryVHGP.Core.Entities
{
    public partial class DeliveryTimeFrame
    {
        public string Id { get; set; } = null!;
        public double? FromHour { get; set; }
        public double? ToHour { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
