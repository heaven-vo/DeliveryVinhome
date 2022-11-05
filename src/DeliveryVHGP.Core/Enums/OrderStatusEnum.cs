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
        CustomerDelivery = 8
    }
    public enum FailStatus
    {
        OutTime = 12,
        StoreFail = 9,
        ShipperFail = 10,
        CustomerFail = 11
    }
}
