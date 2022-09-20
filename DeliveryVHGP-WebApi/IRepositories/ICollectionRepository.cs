using DeliveryVHGP_WebApi.ViewModels;

namespace DeliveryVHGP_WebApi.IRepositories
{
    public interface ICollectionRepository 
    {
        Task<IEnumerable<CollectionModel>> GetAll(int pageIndex, int pageSize);
        Task<CollectionModel> GetById(string Id);
    }
}
