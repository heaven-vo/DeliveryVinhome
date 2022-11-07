namespace DeliveryVHGP.Core.Models
{
    public class CategoryStoreInMenu
    {
        public string Id { get; set; } = null!;
        public string? Image { get; set; }
        public string? Name { get; set; }
        public List<ProductViewInList> ListProducts { get; set; }
    }
    public class StoreInMenuView
    {
        public string Id { get; set; } = null!;
        public string? Image { get; set; }
        public string? Name { get; set; }
        public string Building { get; set; }
        public string StoreCategory { get; set; }

    }
    public class CategoryInMenuView
    {
        public string Id { get; set; } = null!;
        public string? Image { get; set; }
        public string? Name { get; set; }
    }
    public class StoreInProductView
    {
        public string Id { get; set; } = null!;
        public string? Image { get; set; }
        public string? Name { get; set; }
        public string? CloseTime { get; set; }
        public string? OpenTime { get; set; }
        public string? Location { get; set; }
        public List<ProductViewInList> ListProducts { get; set; }
    }
    public class StoreCategoryInMenuView
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public List<StoreInMenuView> ListStores { get; set; }
    }
    public class Request
    {
        public string KeySearch { get; set; } 
        public string SearchBy { get; set; } 
    }
    public class ProductInStoreInMenuVieww
    {
        public string Id { get; set; } = null!;
        public string? Name { get; set; }
        public string? Image { get; set; }
        public List<ProductViewInList> ListProducts { get; set; }

    }
    public class ProductInMenuViewMode3
    {
        public List<MenuMode3Model> menuMode3s { get; set; }
        public List<CategoryInMenuView> categoryInMenuViews { get; set; }
        public List<ProductViewInList> products { get; set; }
    }
    public class MenuMode3Model
    {
        public String Id { get; set; }
        public String? Name { get; set; }
    }
}
