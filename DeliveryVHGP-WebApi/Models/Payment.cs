using System;
using System.Collections.Generic;

namespace DeliveryVHGP_WebApi.Models
{
    public partial class Payment
    {
        public string Id { get; set; } = null!;
        public string? Amount { get; set; }
        public string? Type { get; set; }
        public string? OrderId { get; set; }
        public string? Status { get; set; }

        public virtual Order? Order { get; set; }
    }
}
