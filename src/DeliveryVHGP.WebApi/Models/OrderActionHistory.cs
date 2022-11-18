using System;
using System.Collections.Generic;

namespace DeliveryVHGP.WebApi.Models
{
    public partial class OrderActionHistory
    {
        public string Id { get; set; } = null!;
        public string? OrderId { get; set; }
        public string? TypeId { get; set; }
        public int? FromStatus { get; set; }
        public int? ToStatus { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual Order? Order { get; set; }
        public virtual ActionType? Type { get; set; }
    }
}
