using System;
using System.Collections.Generic;

namespace DeliveryVHGP_WebApi.Models
{
    public partial class Product
    {
        public Product()
        {
            ProductInCollections = new HashSet<ProductInCollection>();
            ProductInMenus = new HashSet<ProductInMenu>();
            ProductTags = new HashSet<ProductTag>();
        }

        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? Unit { get; set; }
        public double? PricePerPack { get; set; }
        public double? PackNetWeight { get; set; }
        public string? PackDescription { get; set; }
        public double? MaximumQuantity { get; set; }
        public double? MinimumQuantity { get; set; }
        public double? MinimumDeIn { get; set; }
        public string? StoreId { get; set; }
        public string? CategoryId { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }

        public virtual Category? Category { get; set; }
        public virtual Store? Store { get; set; }
        public virtual ICollection<ProductInCollection> ProductInCollections { get; set; }
        public virtual ICollection<ProductInMenu> ProductInMenus { get; set; }
        public virtual ICollection<ProductTag> ProductTags { get; set; }
    }
}
