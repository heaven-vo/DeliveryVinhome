using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryVHGP.Core.Enums
{
    public enum OrderStatusEnum
    {
        New = 0,
        Received = 1,
        Assigning = 2,
        Accepted = 3,
        InProcess = 4,
        Completed = 5,
        Fail = 6
    }
    public enum InProcessStatus
    {
        HubDelivery = 7,
        AtHub = 8,
        CustomerDelivery = 9
    }
    public enum FailStatus
    {
        OutTime = 10,
        StoreFail = 11,
        ShipperFail = 12,
        CustomerFail = 13
    }
}
