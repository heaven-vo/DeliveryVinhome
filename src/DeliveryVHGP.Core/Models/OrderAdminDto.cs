using System.ComponentModel.DataAnnotations;

namespace DeliveryVHGP.Core.Models
{
    public class OrderAdminDto
    {
        public string Id { get; set; } = null!;
        public string? StoreName { get; set; }
        public string? BuildingName { get; set; }
        public string? CustomerName { get; set; }
        public string? Phone { get; set; }
        public double? Total { get; set; }
        public string? Note { get; set; }
        public double? ShipCost { get; set; }
        public int? PaymentName { get; set; }
        public int? PaymentStatus { get; set; }
        public string? ModeId { get; set; }
        public int? Status { get; set; }
        public string? Time { get; set; }
        public string? TimeDuration { get; set; }
        public string? FromHour { get; set; }  
        public string? ToHour { get; set; }
        public string? Dayfilter { get; set; }
        //public TimeCreateOrder TimeCreate{ get; set; }
        public class DateFilterRequest
        {
            public string? DateFilter { get; set; } = "";
        }
        public class OrderAdminDtoInStore
        {
            public string Id { get; set; } = null!;
            public string? StoreName { get; set; }
            public string? BuildingName { get; set; }
            public string? CustomerName { get; set; }
            public string? Phone { get; set; }
            public double? Total { get; set; }
            public string? Note { get; set; }
            public double? ShipCost { get; set; }
            public int? PaymentName { get; set; }
            public string? ModeId { get; set; }
            public int? Status { get; set; }
            public DateTime? Time { get; set; }
            public string? CountProduct { get; set; }
        }
        public class CountProduct{
            public string? size { get; set; }


        }

    }
}
