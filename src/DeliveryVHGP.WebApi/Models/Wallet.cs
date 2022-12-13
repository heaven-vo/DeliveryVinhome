using System;
using System.Collections.Generic;

namespace DeliveryVHGP.WebApi.Models
{
    public partial class Wallet
    {
        public Wallet()
        {
            Transactions = new HashSet<Transaction>();
        }

        public string Id { get; set; } = null!;
        public string? AccountId { get; set; }
        public double? Amount { get; set; }
        public bool? Active { get; set; }

        public virtual Account? Account { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
