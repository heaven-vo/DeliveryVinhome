using DeliveryVHGP.Core.Interface.IRepositories;
using DeliveryVHGP.Core.Data;
using DeliveryVHGP.Core.Models;
using Microsoft.EntityFrameworkCore;
using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Infrastructure.Repositories.Common;

namespace DeliveryVHGP.WebApi.Repositories
{
    public class StoreCategoryRepository : RepositoryBase<StoreCategory>, IStoreCategoryRepository
    {

        public StoreCategoryRepository(DeliveryVHGP_DBContext context): base(context)
        {
        }

        public async Task<IEnumerable<StoreCategoryModel>> GetAll(int pageIndex, int pageSize)
        {
            var liststorecate = await context.StoreCategories.
                
               Select(x => new StoreCategoryModel
               {
                   Id = x.Id,
                   Name = x.Name,
                   Status = x.Status
               }).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return liststorecate;
        }
        public async Task<IEnumerable<StoreCategoryModel>> GetStoreCategoryByName(string cateName, int pageIndex, int pageSize)
        {
            var listStoreCategory = await (from Scate in context.StoreCategories
                                  .Where(c => c.Name.Contains(cateName))
                                  select new StoreCategoryModel()
                                  {
                                      Id = Scate.Id,
                                      Name = Scate.Name,
                                      Status = Scate.Status
                                  }).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return listStoreCategory;
        }
        public async Task<StoreCategoryDto> CreateStoreCategory(StoreCategoryDto storeCate)
        {
            context.StoreCategories.Add(new StoreCategory { 
                Id = Guid.NewGuid().ToString(),
                Name = storeCate.Name });
            await context.SaveChangesAsync();
            return storeCate;

        }

        public async Task<Object> DeleteById(string storeCateId)
        {
            var storeCate = await context.StoreCategories.FindAsync(storeCateId);
            context.StoreCategories.Remove(storeCate);
            await context.SaveChangesAsync();

            return storeCateId;

        }
        public async Task<Object> UpdateStoreCateById(string storecaId, StoreCategoryModel storeCate)
        {
            if (storecaId == null)
            {
                return null;
            }
            var result = await context.StoreCategories.FindAsync(storecaId);
            result.Id = storeCate.Id;
            result.Name = storeCate.Name;
            result.Status = storeCate.Status;
            context.Entry(result).State = EntityState.Modified;
            try
            {
                await context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
            return storecaId;
        }
    }
}
