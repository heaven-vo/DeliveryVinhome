namespace DeliveryVHGP_WebApi.ViewModels
{
    public class MenuView
    {
        public String Id { get; set; }
        public String Name { get; set; }
        public String Image { get; set; }
        public double? StartTime { get; set; }
        public double? EndTime { get; set; }
        public List<CategoryStoreInMenu> ListCategoryStoreInMenus { get; set; }
    }
}
