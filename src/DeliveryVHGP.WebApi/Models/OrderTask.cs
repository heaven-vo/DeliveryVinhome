using System;
using System.Collections.Generic;

namespace DeliveryVHGP.WebApi.Models
{
    public partial class OrderTask
    {
        public string? Id { get; set; }
        public string? OrderId { get; set; }
        public string? ShipperId { get; set; }
        public string? Task { get; set; }
        public string? Status { get; set; }

        public virtual Order? Order { get; set; }
        public virtual Shipper? Shipper { get; set; }
    }
}
