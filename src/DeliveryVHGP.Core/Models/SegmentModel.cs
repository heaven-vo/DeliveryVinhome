namespace DeliveryVHGP.Core.Models
{
    public class SegmentModel
    {
        public string SegmentId { get; set; }
        public string OrderId { get; set; }
        public string fromBuilding { get; set; }
        public string toBuilding { get; set; }
        public int? SegmentMode { get; set; }
    }
}
