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
    }
}
