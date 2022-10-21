using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Core.Interfaces;
using DeliveryVHGP.Core.Models;

namespace DeliveryVHGP.Core.Interface.IRepositories
{
    public interface IAreaRepository : IRepositoryBase<Area>
    {
        Task<List<AreaDto>> GetAll(int pageIndex, int pageSize);
        Task<Object> GetBuildingByArea(string areaId);
        Task<AreaModel> CreateArea(AreaModel area);
        Task<Object> UpdateAreaById(string areaId, AreaDto area);
        Task<Object> DeleteById(string areaId);
    }
}
