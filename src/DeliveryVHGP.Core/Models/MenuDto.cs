namespace DeliveryVHGP.Core.Models
{
    public class MenuDto
    {
        //public string Id { get; set; } = null!;
        public string? Image { get; set; }
        public string? Name { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? DayFilter { get; set; }
        public string? HourFilter { get; set; }
        public double? StartHour { get; set; }
        public double? EndHour { get; set; }
        public string? ModeId { get; set; }
        public double? ShipCost { get; set; }
        public int? Priority { get; set; }
        public List<string>? listCategory { get; set; }
    }
}
