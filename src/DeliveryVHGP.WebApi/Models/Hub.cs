using System;
using System.Collections.Generic;

namespace DeliveryVHGP.WebApi.Models
{
    public partial class Hub
    {
        public Hub()
        {
            Buildings = new HashSet<Building>();
            Segments = new HashSet<Segment>();
        }

        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public string? BuildingId { get; set; }

        public virtual ICollection<Building> Buildings { get; set; }
        public virtual ICollection<Segment> Segments { get; set; }
    }
}
