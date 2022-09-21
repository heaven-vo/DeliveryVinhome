using DeliveryVHGP_WebApi.IRepositories;
using DeliveryVHGP_WebApi.Models;
using DeliveryVHGP_WebApi.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DeliveryVHGP_WebApi.Repositories
{
    public class StoreCategoryRepository : IStoreCategoryRepository
    {
        private readonly DeliveryVHGP_DBContext _context;

        public StoreCategoryRepository(DeliveryVHGP_DBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StoreCategoryModel>> GetAll(int pageIndex, int pageSize)
        {
            var liststorecate = await _context.StoreCategories.
               Select(x => new StoreCategoryModel
               {
                   Id = x.Id,
                   Name = x.Name,
                   Status = x.Status
               }).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return liststorecate;
        }
        public async Task<StoreCategoryModel> CreateStoreCategory(StoreCategoryModel storeCate)
        {
            _context.StoreCategories.Add(new StoreCategory { Id = storeCate.Id, Name = storeCate.Name });
            await _context.SaveChangesAsync();
            return storeCate;

        }

        public async Task<Object> DeleteById(string storeCateId)
        {
            var storeCate = await _context.StoreCategories.FindAsync(storeCateId);
            _context.StoreCategories.Remove(storeCate);
            await _context.SaveChangesAsync();

            return storeCateId;

        }

        public async Task<Object> UpdateStoreCateById(string storecaId, StoreCategoryModel storeCate)
        {
            if (storecaId == null)
            {
                return null;
            }
            var result = await _context.StoreCategories.FindAsync(storecaId);
            result.Id = storeCate.Id;
            result.Name = storeCate.Name;

            _context.Entry(result).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
            return storecaId;
        }
    }
}
