using System;
using System.Collections.Generic;

namespace DeliveryVHGP_WebApi.Models
{
    public partial class Menu
    {
        public Menu()
        {
            CategoryInMenus = new HashSet<CategoryInMenu>();
            Orders = new HashSet<Order>();
            ProductInMenus = new HashSet<ProductInMenu>();
        }

        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? DayFilter { get; set; }
        public string? HourFilter { get; set; }
        public string? StartHour { get; set; }
        public string? EndHour { get; set; }
        public string? ModeId { get; set; }

        public virtual DeliveryMode? Mode { get; set; }
        public virtual ICollection<CategoryInMenu> CategoryInMenus { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<ProductInMenu> ProductInMenus { get; set; }
    }
}
