using DeliveryVHGP_WebApi.ViewModels;

namespace DeliveryVHGP_WebApi.IRepositories
{
    public interface IMenuRepository
    {
        Task<List<MenuView>> GetListMenuNow();
        Task<List<MenuView>> GetListMenuByMode(string modeId);
        Task<List<ProductViewInList>> GetListProductInMenu(string id, int page, int pageSize);
        Task<MenuDto> CreatNewMenu(MenuDto menu);
        Task<MenuDto> UpdateMenu(string menuId, MenuDto menu);
    }
}
