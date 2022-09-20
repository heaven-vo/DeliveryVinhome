using System;
using System.Collections.Generic;

namespace DeliveryVHGP_WebApi.Models
{
    public partial class Store
    {
        public Store()
        {
            Collections = new HashSet<Collection>();
            Orders = new HashSet<Order>();
            Products = new HashSet<Product>();
            RequestInMenus = new HashSet<RequestInMenu>();
        }

        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? BuildingId { get; set; }
        public string? BrandId { get; set; }
        public string? Rate { get; set; }
        public string? CloseTime { get; set; }
        public string? OpenTime { get; set; }
        public string? Image { get; set; }
        public string? StoreCategoryId { get; set; }
        public string? Slogan { get; set; }

        public virtual Brand? Brand { get; set; }
        public virtual StoreCategory? StoreCategory { get; set; }
        public virtual ICollection<Collection> Collections { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<RequestInMenu> RequestInMenus { get; set; }
    }
}
