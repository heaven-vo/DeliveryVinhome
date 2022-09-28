using System;
using System.Collections.Generic;

namespace DeliveryVHGP_WebApi.Models
{
    public partial class Category
    {
        public Category()
        {
            CategoryInMenus = new HashSet<CategoryInMenu>();
            Products = new HashSet<Product>();
        }

        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public string? Image { get; set; }

        public virtual ICollection<CategoryInMenu> CategoryInMenus { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
