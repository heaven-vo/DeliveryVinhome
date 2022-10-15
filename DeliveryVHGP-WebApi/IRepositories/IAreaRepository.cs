using DeliveryVHGP_WebApi.ViewModels;

namespace DeliveryVHGP_WebApi.IRepositories
{
    public interface IAreaRepository
    {
        Task<List<AreaDto>> GetAll(int pageIndex, int pageSize);
        Task<Object> GetBuildingByArea(string areaId);
        Task<AreaModel> CreateArea(AreaModel area);
        Task<Object> UpdateAreaById(string areaId, AreaDto area);
        Task<Object> DeleteById(string areaId);
    }
}
