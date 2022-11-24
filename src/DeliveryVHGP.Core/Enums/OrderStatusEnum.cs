namespace DeliveryVHGP.Core.Enums
{
    public enum OrderStatusEnum
    {
        New = 0, // Create order
        Received = 1, // Shop accept(option)
        Assigning = 2, // Add to segment
        Accepted = 3, // Shipper accept a route inclde 1-n order
        InProcess = 4, // Shipper doing at this node( khi nao chuyen qua progress hay skip)
        Completed = 5, // Done
        Fail = 6 // Fail with problem
    }
    public enum InProcessStatus
    {
        HubDelivery = 7, // Doing delivery to hub
        AtHub = 8, // Order in hub
        CustomerDelivery = 9 // Doing delivery to customer
    }
    public enum FailStatus
    {
        OutTime = 10,
        StoreFail = 11,
        ShipperFail = 12,
        CustomerFail = 13
    }
    public enum OrderActionEnum
    {
        PickupStore = 1,
        PickupHub = 2,
        DeliveryHub = 3,
        DeliveryCus = 4
    }
    public enum OrderActionStatusEnum
    {
        Todo = 1,
        Done = 2,
        Fail = 3
    }
}
