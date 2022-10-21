using System;
using System.Collections.Generic;

namespace DeliveryVHGP.Core.Entities
{
    public partial class Role
    {
        public Role()
        {
            Accounts = new HashSet<Account>();
        }

        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public string? Status { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
