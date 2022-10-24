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
    public class StoreCategoryInMenuView
    {
        public string Id { get; set; } = null!;
        public string? Image { get; set; }
        public string? Name { get; set; }
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
    public class StoreInMenuVieww
    {
        public string Id { get; set; } = null!;
        public string? Name { get; set; }
    }
    public class ProductInMenuView
    {
        public string Id { get; set; } = null!;
        public string? Name { get; set; }
    }
}
