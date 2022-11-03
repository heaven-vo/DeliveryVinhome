using System;
using System.Collections.Generic;

namespace DeliveryVHGP.WebApi.Models
{
    public partial class ProductTag
    {
        public string Id { get; set; } = null!;
        public string? ProductId { get; set; }
        public string? TagId { get; set; }

        public virtual Product? Product { get; set; }
        public virtual Tag? Tag { get; set; }
    }
}
