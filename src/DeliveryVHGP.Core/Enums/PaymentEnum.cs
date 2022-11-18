using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryVHGP.Core.Enums
{
    public enum PaymentEnum
    {
        Cash = 0,
        VNPay = 1
    }
    public enum PaymentStatusEnum
    {
        unpaid = 0,
        successful = 1,
        failed = 2
    }
}
