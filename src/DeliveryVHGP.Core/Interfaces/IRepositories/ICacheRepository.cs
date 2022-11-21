using DeliveryVHGP.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryVHGP.Core.Interfaces.IRepositories
{
    public interface ICacheRepository : IRepositoryBase<OrderCache>
    {
        Task AddOrderToCache(List<String> listOrder);
        Task<List<string>> GetOrderFromCache(int size);
    }
}
