using System;
using System.Collections.Generic;

namespace DeliveryVHGP.WebApi.Models
{
    public partial class ProductInCollection
    {
        public string Id { get; set; } = null!;
        public string? CollectionId { get; set; }
        public string? ProductId { get; set; }
        public string? Name { get; set; }

        public virtual Collection? Collection { get; set; }
        public virtual Product? Product { get; set; }
    }
}
