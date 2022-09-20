using DeliveryVHGP_WebApi.ViewModels;

namespace DeliveryVHGP_WebApi.IRepositories
{
    public interface IMenuRepository
    {
        Task<List<MenuView>> GetListMenuName();
        Task<List<MenuView>> GetListMenuNow();
        Task<List<MenuView>> GetListMenuByMode(string modeId);
        Task<List<MenuView>> GetListMenuByStoreId(string storeId);
        Task<List<MenuView>> GetListMenuByCategoryId(string categoryId);
        Task<List<ProductViewInList>> GetListProductInMenu(string id, int page, int pageSize);
        Task<List<ProductViewInList>> GetListProductInMenuByStoreId(string storeId, string menuId, int page, int pageSize);
        Task<List<ProductViewInList>> GetListProductInMenuByCategoryId(string categoryId, string menuId, int page, int pageSize);
        Task<MenuDto> CreatNewMenu(MenuDto menu);
        Task<MenuDto> UpdateMenu(string menuId, MenuDto menu);
    }
}
