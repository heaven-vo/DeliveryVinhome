using System;
using System.Collections.Generic;

namespace DeliveryVHGP_WebApi.Models
{
    public partial class ProductInRequest
    {
        public string Id { get; set; } = null!;
        public string? RequestId { get; set; }
        public string? ProductId { get; set; }
        public double? Price { get; set; }

        public virtual Product? Product { get; set; }
        public virtual RequestInMenu? Request { get; set; }
    }
}
