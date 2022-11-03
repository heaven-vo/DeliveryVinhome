using System;
using System.Collections.Generic;

namespace DeliveryVHGP.WebApi.Models
{
    public partial class Cluster
    {
        public Cluster()
        {
            Buildings = new HashSet<Building>();
        }

        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public string? AreaId { get; set; }

        public virtual Area? Area { get; set; }
        public virtual ICollection<Building> Buildings { get; set; }
    }
}
