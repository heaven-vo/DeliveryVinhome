using DeliveryVHGP_WebApi.ViewModels;

namespace DeliveryVHGP_WebApi.IRepositories
{
    public interface ICollectionRepository 
    {
        Task<IEnumerable<CollectionModel>> GetAll(int pageIndex, int pageSize);
        Task<CollectionModel> CreateCollection(CollectionModel collection);
        Task<Object> UpdateCollectionById(string CollectionId, CollectionModel collection);
    }
}
