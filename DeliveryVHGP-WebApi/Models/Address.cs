using System;
using System.Collections.Generic;

namespace DeliveryVHGP_WebApi.Models
{
    public partial class Address
    {
        public string Id { get; set; } = null!;
        public string? BuildingId { get; set; }
        public string? CustomerId { get; set; }
        public string? Room { get; set; }
        public string? Status { get; set; }
        public string? Name { get; set; }
        public string? Note { get; set; }

        public virtual Building? Building { get; set; }
        public virtual Customer? Customer { get; set; }
    }
}
