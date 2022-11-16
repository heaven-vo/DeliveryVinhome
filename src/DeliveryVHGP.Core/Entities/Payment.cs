using System;
using System.Collections.Generic;

namespace DeliveryVHGP.Core.Entities
{
    public partial class Payment
    {
        public string Id { get; set; } = null!;
        public double? Amount { get; set; }
        public int? Type { get; set; }
        public string? OrderId { get; set; }
        public int? Status { get; set; }

        public virtual Order? Order { get; set; }
    }
}
