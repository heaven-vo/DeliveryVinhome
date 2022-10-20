namespace DeliveryVHGP_WebApi.ViewModels
{
    public class ViewListStoreModel
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? BuildingId { get; set; }
        public string? BrandId { get; set; }
        public string? Rate { get; set; }
        public string? CloseTime { get; set; }
        public string? OpenTime { get; set; }
        public string? Image { get; set; }
        public string? StoreCategoryId { get; set; }
        public string? Slogan { get; set; }
        public string? Phone { get; set; }
        public bool? Status { get; set; }
        public string? CreateAt { get; set; }
        public string? UpdateAt { get; set; }
        public AccountInRole Account { get; set; }
    }
}
