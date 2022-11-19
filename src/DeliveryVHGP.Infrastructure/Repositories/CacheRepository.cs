using DeliveryVHGP.Core.Data;
using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Core.Interfaces.IRepositories;
using DeliveryVHGP.Infrastructure.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryVHGP.Infrastructure.Repositories
{
    public class CacheRepository : RepositoryBase<OrderCache>, ICacheRepository
    {
        public CacheRepository(DeliveryVHGP_DBContext context) : base(context)
        {
        }
        public async Task AddOrderToCache(List<string> listOrder)
        {
            List<OrderCache> list = new List<OrderCache>();
            foreach(var orderId in listOrder)
            {
                OrderCache cache = new OrderCache() { Id = Guid.NewGuid().ToString(), OrderId = orderId, CreateAt = DateTime.UtcNow.AddHours(7), UpdateAt = DateTime.UtcNow.AddHours(7) };
                list.Add(cache);
            }
            await AddRange(list);
            await Save();

        }
    }
}
