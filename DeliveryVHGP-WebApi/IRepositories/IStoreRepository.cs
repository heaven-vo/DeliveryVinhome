using DeliveryVHGP_WebApi.ViewModels;

namespace DeliveryVHGP_WebApi.IRepositories
{
    public interface IStoreRepository
    {
        Task<IEnumerable<StoreModel>> GetAll(int pageIndex, int pageSize);
    }
}
