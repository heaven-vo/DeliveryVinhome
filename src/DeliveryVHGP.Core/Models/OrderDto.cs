namespace DeliveryVHGP.Core.Models
{
    public class OrderDto
    {
        public string Id { get; set; } = null!;
        //public string? CustomerId { get; set; }
        public string? PhoneNumber { get; set; }
        public double? Total { get; set; }
        public string? StoreId { get; set; }
        public string? MenuId { get; set; }
        public string? BuildingId { get; set; }
        public string? Note { get; set; }
        public string? FullName { get; set; }
        public double? ShipCost { get; set; }
        public string? DeliveryTimeId { get; set; }

        public List<OrderDetailDto> OrderDetail { get; set; }
        public List<PaymentDto> Payments { get; set; }


    }
    public class OrderInfor
    {
        public string OrderId { get; set; }
        public double Amount { get; set; }
        public int Status { get; set; }
        //public string PaymentId { get; set; }
    }
    public class ResponePayment
    {
        public string vnp_Amount { get; set; }
        public string vnp_BankCode { get; set; }
        public string vnp_BankTranNo { get; set; }
        public string vnp_CardType { get; set; }
        public string vnp_OrderInfo { get; set; }
        public string vnp_PayDate { get; set; }
        public string vnp_ResponseCode { get; set; }
        public string vnp_TmnCode { get; set; }
        public string vnp_TransactionNo { get; set; }
        public string vnp_TransactionStatus { get; set; }
        public string vnp_TxnRef { get; set; }
        public string vnp_SecureHash { get; set; }
    }
}
