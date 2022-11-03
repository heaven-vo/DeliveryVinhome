using System;
using System.Collections.Generic;

namespace DeliveryVHGP.WebApi.Models
{
    public partial class Collection
    {
        public Collection()
        {
            ProductInCollections = new HashSet<ProductInCollection>();
        }

        public string Id { get; set; } = null!;
        public string? StoreId { get; set; }
        public string? Name { get; set; }

        public virtual Store? Store { get; set; }
        public virtual ICollection<ProductInCollection> ProductInCollections { get; set; }
    }
}
