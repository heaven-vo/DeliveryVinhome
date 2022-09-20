using DeliveryVHGP_WebApi.IRepositories;
using DeliveryVHGP_WebApi.Models;
using DeliveryVHGP_WebApi.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DeliveryVHGP_WebApi.Repositories
{
    public class BrandRepository : IBrandRepository

    {
        private readonly DeliveryVHGP_DBContext _context;

        public BrandRepository (DeliveryVHGP_DBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BrandModels>> GetAll(int pageIndex, int pageSize)
        {
            var listbrand = await _context.Brands.
                Select(x => new BrandModels
            {
                Id = x.Id,
                Name = x.Name,
                Image = x.Image
            }).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return listbrand;
        }

        public async Task<BrandModels>GetById(string id)
        {
            var brand = await _context.Brands.Where(x => x.Id == id).Select(x => new BrandModels
            {
                Id=x.Id,
                Name=x.Name,
                Image=x.Image
            }).FirstOrDefaultAsync();
            return brand;
        }
    }
}
