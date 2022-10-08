using DeliveryVHGP_WebApi.ViewModels;

namespace DeliveryVHGP_WebApi.IRepositories
{
    public interface IBuildingRepository
    {
        Task<List<ViewListBuilding>> GetAll(int pageIndex, int pageSize);
        Task<BuildingModel> CreateBuilding(BuildingModel building);
    }
}
