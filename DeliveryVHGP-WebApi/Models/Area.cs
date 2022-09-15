using System;
using System.Collections.Generic;

namespace DeliveryVHGP_WebApi.Models
{
    public partial class Area
    {
        public string? Id { get; set; }
        public string? Name { get; set; }

        public virtual Building? IdNavigation { get; set; }
    }
}
