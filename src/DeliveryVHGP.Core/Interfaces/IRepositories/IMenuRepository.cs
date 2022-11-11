using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Core.Interfaces;
using DeliveryVHGP.Core.Models;

namespace DeliveryVHGP.Core.Interface.IRepositories
{
    public interface IMenuRepository : IRepositoryBase<Menu>
    {
        Task<MenuDto> GetMenuDetail(string menuId);
        Task<List<MenuView>> GetListMenuByModeId(string modeId);
        Task<MenuViewModel> Filter(string KeySearch, string menuId, int page, int pageSize); //search
        Task<List<ProductInStoreInMenuVieww>> GetListProductInStoreInMenuByName(string KeySearch, string menuId, int page, int pageSize); // search show list product in store
        Task<MenuNotProductView> GetMenuByModeAndShowListCategory(string modeId);
        Task<List<StoreCategoryInMenuView>> GetListStoreCateInMenuNow(string modeId, int storeCateSize, int storeSize);
        Task<List<StoreInMenuView>> GetListStoreInMenuNow(string modeId, int page, int pageSize);
        Task<MenuView> GetMenuByModeAndGroupByStore(string modeId, int page, int pageSize);
        Task<MenuView> GetMenuByModeAndGroupByCategory(string modeId, int page, int pageSize) ;
        Task<List<MenuViewMode3>> GetListMenuInMode3(int pageSize); // mode 3
        Task<StoreInMenuViewMode3> GetListStoreInMenuMode3(string menuId, PagingRequest request);//a menu mode 3
        Task<List<CategoryStoreInMenu>> GetMenuByMenuIdAndStoreIdAndGroupByCategory(string menuId, string storeId, int page, int pageSize);
        Task<List<StoreInMenuView>> GetListStoreInMenuFilerByCategory(string menuId, string categoryId, int page, int pageSize);
        Task<StoreInProductView> GetAllProductInMenuByStoreId(string storeId, string menuId, int page, int pageSize);
        Task<CategoryStoreInMenu> GetAllProductInMenuByCategoryId(string categoryId, string menuId, int page, int pageSize);
        Task<CategoryStoreInMenu> GetAllProductInMenuByCategoryIdAndStoreId(string storeId, string categoryId, string menuId, int page, int pageSize);
        Task<List<ProductViewInList>> GetListProductInMenuByStoreId(string storeId, string menuId, int page, int pageSize);
        Task<List<ProductViewInList>> GetListProductInMenuByCategoryId(string categoryId, string menuId, int page, int pageSize);
        Task<List<ProductViewInList>> GetListProductNotInMenuByCategoryIdAndStoreId(string storeId, string menuId, int page, int pageSize);
        Task<ProductsInMenuModel> AddProductsToMenu(ProductsInMenuModel listProduct);
        Task<MenuDto> CreatNewMenu(MenuDto menu);
        Task<MenuDto> UpdateMenu(string menuId, MenuDto menu);

    }
}
