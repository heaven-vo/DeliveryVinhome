using DeliveryVHGP.Core.Data;
using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Core.Interfaces.IRepositories;
using DeliveryVHGP.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;
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
                OrderCache cache = new OrderCache() { Id = Guid.NewGuid().ToString(), OrderId = orderId, CreateAt = DateTime.UtcNow.AddHours(7), UpdateAt = DateTime.UtcNow.AddHours(7), IsReady = true };
                list.Add(cache);
            }
            try
            {
                await context.OrderCaches.AddRangeAsync(list);
                await Save();
            }
            catch
            {
                throw new Exception("Order duplicate");
            }

        }
        public async Task<List<string>> GetOrderFromCache(int size)
        {
            var listOrer = await context.OrderCaches.Where(x => x.IsReady == true).OrderBy(x => x.CreateAt).Select(x => x.OrderId).Take(size).ToListAsync();
            return listOrer;
        }
    }
}
