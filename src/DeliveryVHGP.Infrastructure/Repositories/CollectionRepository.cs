using DeliveryVHGP.Core.Interface.IRepositories;
using DeliveryVHGP.Core.Data;
using DeliveryVHGP.Core.Models;
using Microsoft.EntityFrameworkCore;
using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Infrastructure.Repositories.Common;

namespace DeliveryVHGP.WebApi.Repositories
{
    public class CollectionRepository : RepositoryBase<Collection>, ICollectionRepository
    {

        public CollectionRepository(DeliveryVHGP_DBContext context): base(context)
        {
        }

        public async Task<IEnumerable<CollectionModel>> GetAll(int pageIndex, int pageSize)
        {
            var listCollection = await context.Collections.
                Select(x => new CollectionModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    StoreId = x.StoreId
                }).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return listCollection;
        }
        public async Task<CollectionModel> CreateCollection(CollectionModel collection)
        {
            context.Collections.Add(new Collection { Id = collection.Id, Name = collection.Name });
            await context.SaveChangesAsync();
            return collection;

        }
        public async Task<Object> UpdateCollectionById(string CollectionId, CollectionModel collection)
        {
            if (CollectionId == null)
            {
                return null;
            }
            var result = await context.Collections.FindAsync(CollectionId);
            result.Id = collection.Id;
            result.Name = collection.Name;

            context.Entry(result).State = EntityState.Modified;
            try
            {
                await context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
            return collection;
        }
    }
}
