namespace DeliveryVHGP.Core.Models
{
    public class ShipperReportModel
    {
        public int total { get; set; }
        public int success { get; set; }
        public int canceled { get; set; }
        public int customerFail { get; set; }
    }
    public class DeliveryShipperReportModel
    {
        public int total { get; set; }
        public int success { get; set; }
        public int cancel { get; set; }
        public List<ShipperInReport> shipperInReports { get; set; }
    }
    public class ShipperInReport
    {
        public string? fullname { get; set; }
        public string? phone { get; set; }
        public double? distance { get; set; }
        public int totalOrder { get; set; }
        public int successfulOrder { get; set; }
        public int canceledOrder { get; set; }
        public double? refundBalance { get; set; }
        public double? debitBalance { get; set; }

    }
}
