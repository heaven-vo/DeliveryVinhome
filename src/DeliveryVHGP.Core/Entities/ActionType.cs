using System;
using System.Collections.Generic;

namespace DeliveryVHGP.Core.Entities
{
    public partial class ActionType
    {
        public ActionType()
        {
            OrderActionHistories = new HashSet<OrderActionHistory>();
        }

        public string Id { get; set; } = null!;
        public string? Name { get; set; }

        public virtual ICollection<OrderActionHistory> OrderActionHistories { get; set; }
    }
}
