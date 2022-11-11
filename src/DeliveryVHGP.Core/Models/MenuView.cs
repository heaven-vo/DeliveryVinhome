namespace DeliveryVHGP.Core.Models
{
    public class MenuView
    {
        public String Id { get; set; }
        public String Name { get; set; }
        public String Image { get; set; }
        public String? DayFilter { get; set; }
        public double? StartTime { get; set; }
        public double? EndTime { get; set; }
        public List<CategoryStoreInMenu> ListCategoryStoreInMenus { get; set; }
    }
    public class MenuNotProductView
    {
        public String Id { get; set; }
        public String Name { get; set; }    
        public String Image { get; set; }
        public double? StartTime { get; set; }
        public double? EndTime { get; set; }
        public List<CategoryInMenuView> ListCategoryInMenus { get; set; }
    }
    public class MenuViewMode3
    {
        public String Id { get; set; }
        public String Name { get; set; }
        public String Image { get; set; }
        public String? DayFilter { get; set; }
        public List<StoreInMenuView> ListStores { get; set; }
    }
    public class MenuViewModel
    {
        //public int? Count { get; set; }

        public List<ProductViewInList> Product { get; set; }
        public List<StoreInMenuView> Store { get; set; }
    }
}
