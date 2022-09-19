namespace DeliveryVHGP_WebApi.ViewModels
{
    public class MenuDto
    {
        public string Id { get; set; } = null!;
        public string? Image { get; set; }
        public string? Name { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? DayFilter { get; set; }
        public string? HourFilter { get; set; }
        public double? StartHour { get; set; }
        public double? EndHour { get; set; }
        public string? ModeId { get; set; }
    }
}
