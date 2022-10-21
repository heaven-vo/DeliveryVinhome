using DeliveryVHGP.Core.Interface.IRepositories;
using DeliveryVHGP.Core.Data;
using DeliveryVHGP.Core.Models;
using Microsoft.EntityFrameworkCore;
using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Infrastructure.Repositories.Common;

namespace DeliveryVHGP.WebApi.Repositories
{
    public class BrandRepository : RepositoryBase<Brand>, IBrandRepository

    {
        public BrandRepository (DeliveryVHGP_DBContext context) : base (context)
        {
        }

        public async Task<IEnumerable<BrandModels>> GetAll(int pageIndex, int pageSize)
        {
            var listbrand = await context.Brands.
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
            var brand = await context.Brands.Where(x => x.Id == id).Select(x => new BrandModels
            {
                Id=x.Id,
                Name=x.Name,
                Image=x.Image
            }).FirstOrDefaultAsync();
            return brand;
        }
        public async Task<BrandModels> CreateBrand(BrandModels brand)
        {
            context.Brands.Add(new Brand { Id = brand.Id, Name = brand.Name, Image = brand.Image });
            await context.SaveChangesAsync();
            return brand;

        }

        public async Task<Object> DeleteById(string brandId)
        {
            var brand = await context.Brands.FindAsync(brandId);
            context.Brands.Remove(brand);
            await context.SaveChangesAsync();

            return brand;

        }

        public async Task<Object> UpdateBrandById(string brandId, BrandModels brand)
        {
            if (brandId == null)
            {
                return null;
            }
            var result = await context.Brands.FindAsync(brandId);
            result.Id = brand.Id;
            result.Name = brand.Name;
            result.Image = brand.Image;

            context.Entry(result).State = EntityState.Modified;
            try
            {
                await context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
            return brand;
        }
    }
}
