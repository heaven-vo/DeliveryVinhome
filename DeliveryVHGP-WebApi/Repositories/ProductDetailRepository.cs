using DeliveryVHGP_WebApi.IRepositories;
using DeliveryVHGP_WebApi.Models;
using DeliveryVHGP_WebApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeliveryVHGP_WebApi.Repositories
{
    public class ProductDetailRepository : IProductv1Repository
    {
        private readonly DeliveryVHGP_DBContext _context;

        public ProductDetailRepository(DeliveryVHGP_DBContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ProductDetailsModel>> GetAll(int pageIndex, int pageSize)
        {
            var listproductdetail = await (from p in _context.Products
                                           join s in _context.Stores on p.StoreId equals s.Id
                                           join c in _context.Categories on p.CategoryId equals c.Id
                                           select new ProductDetailsModel()
                                           {
                                               Id = p.Id,
                                               Name = p.Name,
                                               Image = p.Image,
                                               Unit = p.Unit,
                                               PricePerPack = p.PricePerPack,
                                               PackNetWeight = p.PackNetWeight,
                                               PackDescription = p.PackDescription,
                                               MaximumQuantity = p.MaximumQuantity,
                                               MinimumQuantity = p.MinimumQuantity,
                                               Description = p.Description,
                                               Rate = p.Rate,
                                               StoreId = s.Id,
                                               StoreName = s.Name,
                                               StoreImage = s.Image,
                                               Slogan = s.Slogan,
                                               CategoryId = c.Id,
                                               ProductCategory = c.Name
                                           }
                                     ).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return listproductdetail;
        }

        public async Task<Object> UpdateProductDetailById(string proId, ProductDetailsModel product)
        {
            if (proId == null)
            {
                return null;
            }
            var pro = await _context.Products.FindAsync(proId);
            var store = _context.Stores.Where(x => x.Name == product.StoreName).Where(x => x.Image == product.StoreImage).Select(x => x.Id).FirstOrDefault();
            var category = _context.Categories.Where(c => c.Name == product.ProductCategory).Select(c => c.Id).FirstOrDefault();
            if (store == null)
                return null;
            if (category == null)
                return null;
                pro.Id = product.Id;
                pro.Name = product.Name;
                pro.Image = product.Image;
                pro.Unit = product.Unit;
                pro.PricePerPack = product.PricePerPack;
                pro.PackNetWeight = product.PackNetWeight;
                pro.PackDescription = product.PackDescription;
                pro.MaximumQuantity = product.MaximumQuantity;
                pro.MinimumQuantity = product.MinimumQuantity;
                pro.Description = product.Description;
                pro.Rate = product.Rate;
                pro.StoreId = store.ToString();
                pro.CategoryId = category.ToString();
                _context.Entry(pro).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
            return product;
        }
        public async Task<Object> DeleteProductById(string id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return null;
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return product;
        }
    }
}
