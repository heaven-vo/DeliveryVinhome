using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryVHGP.Core.Models
{
    public class TimeDurationOrder
    {
        public string Id { get; set; } = null!;
        public string? MenuId { get; set; }
        public string? FromHour { get; set; }
        public string? ToHour { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
