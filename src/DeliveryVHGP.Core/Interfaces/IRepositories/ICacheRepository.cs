using DeliveryVHGP.Core.Entities;

namespace DeliveryVHGP.Core.Interfaces.IRepositories
{
    public interface ICacheRepository : IRepositoryBase<OrderCache>
    {
        Task AddOrderToCache(List<String> listOrder);
        Task<List<string>> GetOrderFromCache(int size, int mode);
    }
}
