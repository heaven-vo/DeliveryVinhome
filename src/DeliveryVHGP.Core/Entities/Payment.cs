using System;
using System.Collections.Generic;

namespace DeliveryVHGP.Core.Entities
{
    public partial class Payment
    {
        public string Id { get; set; } = null!;
        public double? Amount { get; set; }
        public string? Type { get; set; }
        public string? OrderId { get; set; }
        public string? Status { get; set; }
        public string? Url { get; set; }

        public virtual Order? Order { get; set; }
    }
}
