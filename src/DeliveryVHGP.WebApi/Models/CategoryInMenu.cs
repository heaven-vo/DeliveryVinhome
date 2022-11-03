using System;
using System.Collections.Generic;

namespace DeliveryVHGP.WebApi.Models
{
    public partial class CategoryInMenu
    {
        public string Id { get; set; } = null!;
        public string? MenuId { get; set; }
        public string? CategoryId { get; set; }

        public virtual Category? Category { get; set; }
        public virtual Menu? Menu { get; set; }
    }
}
