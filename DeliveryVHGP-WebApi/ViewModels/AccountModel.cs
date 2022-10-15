namespace DeliveryVHGP_WebApi.ViewModels
{
    public class AccountModel
    {
        public string Id { get; set; } = null!;
        public string? Password { get; set; }
        public string? Name { get; set; }
        public string? RoleId { get; set; }
        public string? Status { get; set; }
    }
}
