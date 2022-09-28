using System;
using System.Collections.Generic;

namespace DeliveryVHGP_WebApi.Models
{
    public partial class ProductInMenu
    {
        public ProductInMenu()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public string Id { get; set; } = null!;
        public double? Price { get; set; }
        public string? MenuId { get; set; }
        public string? ProductId { get; set; }
        public string? Status { get; set; }

        public virtual Menu? Menu { get; set; }
        public virtual Product? Product { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
