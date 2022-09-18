using DeliveryVHGP_WebApi.ViewModels;

namespace DeliveryVHGP_WebApi.IRepositories
{
    public interface IMenuRepository
    {
        Task<List<MenuView>> getListMenuNow();
        Task<List<ProductViewInList>> getListProductInMenu(string id, int page, int pageSize);
    }
}
