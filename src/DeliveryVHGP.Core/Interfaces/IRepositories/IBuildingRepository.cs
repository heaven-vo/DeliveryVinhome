using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Core.Interfaces;
using DeliveryVHGP.Core.Models;

namespace DeliveryVHGP.Core.Interface.IRepositories
{
    public interface IBuildingRepository : IRepositoryBase<Building>
    {
        Task<List<ViewListBuilding>> GetAll(int pageIndex, int pageSize);
        Task<BuildingModel> CreateBuilding(BuildingModel building);
        //Task<Object> GetBuildingByArea(string areaId);
    }
}
