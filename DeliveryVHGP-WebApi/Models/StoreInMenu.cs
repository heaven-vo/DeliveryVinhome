using System;
using System.Collections.Generic;

namespace DeliveryVHGP_WebApi.Models
{
    public partial class StoreInMenu
    {
        public string? Id { get; set; }
        public string? StoreId { get; set; }
        public string? MenuId { get; set; }

        public virtual Menu? Menu { get; set; }
        public virtual Store? Store { get; set; }
    }
}
