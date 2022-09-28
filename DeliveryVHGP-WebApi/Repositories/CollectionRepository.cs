using DeliveryVHGP_WebApi.IRepositories;
using DeliveryVHGP_WebApi.Models;
using DeliveryVHGP_WebApi.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DeliveryVHGP_WebApi.Repositories
{
    public class CollectionRepository : ICollectionRepository
    {
        private readonly DeliveryVHGP_DBContext _context;

        public CollectionRepository(DeliveryVHGP_DBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CollectionModel>> GetAll(int pageIndex, int pageSize)
        {
            var listCollection = await _context.Collections.
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
            _context.Collections.Add(new Collection { Id = collection.Id, Name = collection.Name });
            await _context.SaveChangesAsync();
            return collection;

        }
        public async Task<Object> UpdateCollectionById(string CollectionId, CollectionModel collection)
        {
            if (CollectionId == null)
            {
                return null;
            }
            var result = await _context.Collections.FindAsync(CollectionId);
            result.Id = collection.Id;
            result.Name = collection.Name;

            _context.Entry(result).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
            return collection;
        }
    }
}
