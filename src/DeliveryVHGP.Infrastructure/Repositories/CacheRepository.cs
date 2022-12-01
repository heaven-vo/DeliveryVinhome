using DeliveryVHGP.Core.Data;
using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Core.Enums;
using DeliveryVHGP.Core.Interfaces.IRepositories;
using DeliveryVHGP.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace DeliveryVHGP.Infrastructure.Repositories
{
    public class CacheRepository : RepositoryBase<OrderCache>, ICacheRepository
    {
        public CacheRepository(DeliveryVHGP_DBContext context) : base(context)
        {
        }
        public async Task AddOrderToCache(List<string> listOrderId)
        {
            List<OrderCache> list = new List<OrderCache>();
            foreach (var orderId in listOrderId)
            {
                OrderCache cache = new OrderCache() { Id = Guid.NewGuid().ToString(), OrderId = orderId, CreateAt = DateTime.UtcNow.AddHours(7), UpdateAt = DateTime.UtcNow.AddHours(7), IsReady = true };
                list.Add(cache);
                var actionHistory = new OrderActionHistory()
                {
                    Id = Guid.NewGuid().ToString(),
                    OrderId = orderId,
                    FromStatus = (int)OrderStatusEnum.Received,
                    ToStatus = (int)OrderStatusEnum.Assigning,
                    CreateDate = DateTime.UtcNow.AddHours(7),
                    TypeId = "1"
                };
                await context.OrderActionHistories.AddAsync(actionHistory);
            }
            var listOrder = await context.Orders.Where(x => listOrderId.Contains(x.Id)).ToListAsync();
            listOrder.ForEach(x => x.Status = (int)OrderStatusEnum.Assigning);
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
