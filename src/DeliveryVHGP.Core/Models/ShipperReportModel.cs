namespace DeliveryVHGP.Core.Models
{
    public class ShipperReportModel
    {
        public int total { get; set; }
        public int success { get; set; }
        public int canceled { get; set; }
        public int customerFail { get; set; }
    }
}
