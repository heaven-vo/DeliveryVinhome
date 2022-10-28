using System;
using System.Collections.Generic;

namespace DeliveryVHGP.Core.Entities
{
    public partial class OrderActionHistory
    {
        public string Id { get; set; } = null!;
        public string? OrderId { get; set; }
        public string? TypeId { get; set; }
        public string? FromStatus { get; set; }
        public string? ToStatus { get; set; }
        public string? CreateDate { get; set; }

        public virtual Order? Order { get; set; }
        public virtual ActionType? Type { get; set; }
    }
}
