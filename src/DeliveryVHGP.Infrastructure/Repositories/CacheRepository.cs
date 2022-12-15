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
            List<OrderCache> listCaches = new List<OrderCache>();
            var listOrder = await context.Orders.Where(x => listOrderId.Contains(x.Id)).ToListAsync();
            listOrder.ForEach(x => x.Status = (int)OrderStatusEnum.Assigning);

            foreach (var order in listOrder)
            {
                var menu = await context.Menus.Where(x => x.Id == order.MenuId).FirstOrDefaultAsync();
                OrderCache cache = new OrderCache() { Id = Guid.NewGuid().ToString(), OrderId = order.Id, MenuSaleMode = int.Parse(menu.SaleMode), CreateAt = DateTime.UtcNow.AddHours(7), UpdateAt = DateTime.UtcNow.AddHours(7), IsReady = true };
                listCaches.Add(cache);
                var actionHistory = new OrderActionHistory()
                {
                    Id = Guid.NewGuid().ToString(),
                    OrderId = order.Id,
                    FromStatus = (int)OrderStatusEnum.Received,
                    ToStatus = (int)OrderStatusEnum.Assigning,
                    CreateDate = DateTime.UtcNow.AddHours(7),
                    TypeId = "1"
                };
                await context.OrderActionHistories.AddAsync(actionHistory);
            }
            try
            {
                await context.OrderCaches.AddRangeAsync(listCaches);
                await Save();
            }
            catch
            {
                throw new Exception("Order duplicate");
            }

        }
        public async Task<List<string>> GetOrderFromCache(int size, int mode)
        {
            List<string> listOrerId = new List<string>();
            if (mode == 1)
            {
                listOrerId = await context.OrderCaches.Where(x => x.IsReady == true && x.MenuSaleMode == 1)
                    .OrderBy(x => x.CreateAt).Select(x => x.OrderId).Take(size).ToListAsync();
            }
            if (mode != 1)
            {
                listOrerId = await context.OrderCaches.Where(x => x.IsReady == true && x.MenuSaleMode != 1)
                    .OrderBy(x => x.CreateAt).Select(x => x.OrderId).Take(size).ToListAsync();
            }
            return listOrerId;
        }
    }
}
