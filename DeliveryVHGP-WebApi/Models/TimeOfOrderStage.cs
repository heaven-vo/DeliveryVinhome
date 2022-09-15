using System;
using System.Collections.Generic;

namespace DeliveryVHGP_WebApi.Models
{
    public partial class TimeOfOrderStage
    {
        public string Id { get; set; } = null!;
        public string? Time { get; set; }
        public string? OrderId { get; set; }
        public string? StatusId { get; set; }

        public virtual OrderStatus? Status { get; set; }
    }
}
