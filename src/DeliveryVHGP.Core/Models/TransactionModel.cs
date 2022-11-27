namespace DeliveryVHGP.Core.Models
{
    public class TransactionModel
    {
        public int? TransactionType { get; set; }
        public double? Amount { get; set; }
        public int? TransactionAction { get; set; }
        public DateTime? Date { get; set; }
        public int? Status { get; set; }
    }
}
