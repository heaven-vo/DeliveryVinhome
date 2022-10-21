using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Core.Interfaces;
using DeliveryVHGP.Core.Models;

namespace DeliveryVHGP.Core.Interface.IRepositories
{
    public interface ICollectionRepository : IRepositoryBase<Collection>
    {
        Task<IEnumerable<CollectionModel>> GetAll(int pageIndex, int pageSize);
        Task<CollectionModel> CreateCollection(CollectionModel collection);
        Task<Object> UpdateCollectionById(string CollectionId, CollectionModel collection);
    }
}
