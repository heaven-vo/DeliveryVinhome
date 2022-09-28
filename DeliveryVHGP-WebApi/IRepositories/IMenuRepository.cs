using DeliveryVHGP_WebApi.ViewModels;

namespace DeliveryVHGP_WebApi.IRepositories
{
    public interface IMenuRepository
    {
        Task<MenuDto> GetMenuDetail(string menuId);
        Task<List<MenuView>> GetListMenuName();
        Task<MenuView> GetMenuByModeAndGroupByStore(string modeId, int page, int pageSize);
        Task<MenuView> GetMenuByModeAndGroupByCategory(string modeId, int page, int pageSize) ;
        Task<List<CategoryStoreInMenu>> GetMenuByMenuIdAndStoreIdAndGroupByCategory(string menuId, string storeId, int page, int pageSize);
        Task<List<ProductViewInList>> GetListProductInMenuByStoreId(string storeId, string menuId, int page, int pageSize);
        Task<List<ProductViewInList>> GetListProductInMenuByCategoryId(string categoryId, string menuId, int page, int pageSize);
        Task<List<ProductViewInList>> GetListProductNotInMenuByCategoryIdAndStoreId(string storeId, string menuId, int page, int pageSize);
        //Task<List<ProductViewInList>> GetListProductNotMenuByStoreId(string storeId, string menuId, int page, int pageSize);
        Task<MenuDto> CreatNewMenu(MenuDto menu);
        Task<MenuDto> UpdateMenu(string menuId, MenuDto menu);

    }
}
