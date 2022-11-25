using System;
using System.Collections.Generic;

namespace DeliveryVHGP.Core.Entities
{
    public partial class Transaction
    {
        public string Id { get; set; } = null!;
        public string? WalletId { get; set; }
        public string? OrderId { get; set; }
        public double? Amount { get; set; }
        public int? Action { get; set; }
        public int? Type { get; set; }
        public DateTime? CreateAt { get; set; }
        public int? Status { get; set; }

        public virtual Order? Order { get; set; }
        public virtual Wallet? Wallet { get; set; }
    }
}
