namespace DeliveryVHGP.Core.Enums
{
    public enum TransactionTypeEnum
    {
        refund = 1,     //hoàn trả
        cod = 2,        //phí thu hộ
        shippingcost = 3,
        recharge = 4,   //nạp
        withdraw = 5    //rút
    }
    public enum TransactionActionEnum
    {
        plus = 1,
        minus = 2
    }
}
