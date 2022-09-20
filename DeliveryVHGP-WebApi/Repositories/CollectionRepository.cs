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
            foreach (CollectionModel collection in listCollection)
            {
                collection.Store = await GetCollectionByStore(collection.Id);
                collection.ListProductInCollections = await GetCollectionByProduct(collection.Id);
            }
            return listCollection;
        }

        public async Task<CollectionModel> GetById(string Id)
        {
            var listCollection = await _context.Collections.Where(x => x.Id == Id).Select(x => new CollectionModel
            {
                Id = x.Id,
                Name = x.Name,
                StoreId = x.StoreId
            }).FirstOrDefaultAsync();
            if (listCollection != null)
            {
                listCollection.Store = await GetCollectionByStore(listCollection.Id);
                listCollection.ListProductInCollections = await GetCollectionByProduct(listCollection.Id);
            }
            return listCollection;
        }

        public async Task<List<string>> GetCollectionByStore(string storeId)
        {
            List<string> Store = await (from c in _context.Collections
                                                      join s in _context.Stores on c.StoreId equals s.Id
                                                     
                                                      where c.Id == storeId
                                                      select s.Name
                              ).ToListAsync();
            return Store;
        }

        public async Task<List<string>> GetCollectionByProduct(string proId)
        {
            List<string> ListProductInCollections = await (from c in _context.Collections
                                        join pc in _context.ProductInCollections on c.Id equals pc.CollectionId
                                        join p in _context.Products on pc.ProductId equals p.Id

                                        where c.Id == proId
                                        select p.Name
                              ).ToListAsync();
            return ListProductInCollections;
        }
    }
}
