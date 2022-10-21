using System;
using System.Collections.Generic;

namespace DeliveryVHGP.Core.Entities
{
    public partial class DeliveryMode
    {
        public DeliveryMode()
        {
            Menus = new HashSet<Menu>();
        }

        public string Id { get; set; } = null!;
        public string? Name { get; set; }

        public virtual ICollection<Menu> Menus { get; set; }
    }
}
