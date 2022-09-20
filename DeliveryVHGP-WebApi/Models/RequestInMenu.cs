using System;
using System.Collections.Generic;

namespace DeliveryVHGP_WebApi.Models
{
    public partial class RequestInMenu
    {
        public RequestInMenu()
        {
            ProductInRequests = new HashSet<ProductInRequest>();
        }

        public string Id { get; set; } = null!;
        public string MenuId { get; set; } = null!;
        public string? StoreId { get; set; }
        public string? Status { get; set; }

        public virtual Menu Menu { get; set; } = null!;
        public virtual Store? Store { get; set; }
        public virtual ICollection<ProductInRequest> ProductInRequests { get; set; }
    }
}
