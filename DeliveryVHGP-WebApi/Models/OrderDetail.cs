using System;
using System.Collections.Generic;

namespace DeliveryVHGP_WebApi.Models
{
    public partial class OrderDetail
    {
        public string Id { get; set; } = null!;
        public string? ProductInMenuId { get; set; }
        public string? OrderId { get; set; }
        public string? Quantity { get; set; }
        public double? Price { get; set; }
        public string? ProductName { get; set; }
        public string? ProductId { get; set; }

        public virtual Order? Order { get; set; }
        public virtual Product? Product { get; set; }
        public virtual ProductInMenu? ProductInMenu { get; set; }
    }
}
