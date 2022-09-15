using System;
using System.Collections.Generic;

namespace DeliveryVHGP_WebApi.Models
{
    public partial class DeliveryShiftOfShipper
    {
        public string Id { get; set; } = null!;
        public string? ShipperId { get; set; }
        public string? ShiftId { get; set; }
        public string? ScheduleId { get; set; }
        public string? TaskType { get; set; }
        public string? HubId { get; set; }

        public virtual Hub? Hub { get; set; }
        public virtual Schedule? Schedule { get; set; }
        public virtual Shift? Shift { get; set; }
        public virtual Shipper? Shipper { get; set; }
    }
}
