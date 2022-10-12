using DeliveryVHGP_WebApi.IRepositories;
using DeliveryVHGP_WebApi.Models;
using DeliveryVHGP_WebApi.ViewModels;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DeliveryVHGP_WebApi.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IFileService _fileService;
        private readonly DeliveryVHGP_DBContext context;
        public ProductRepository(IFileService fileService, DeliveryVHGP_DBContext context)
        {
            this.context = context;
            _fileService = fileService;
        }
        public async Task<IEnumerable<ProductDetailsModel>> GetAll(string storeId,int pageIndex, int pageSize)
        {
            var listproductdetail = await (from p in context.Products
                                           join s in context.Stores on p.StoreId equals s.Id
                                           join c in context.Categories on p.CategoryId equals c.Id
                                           where s.Id == storeId
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
                                               MinimumDeIn = p.MinimumDeIn,
                                               Rate = p.Rate,
                                               StoreId = s.Id,
                                               StoreName = s.Name,
                                               StoreImage = s.Image,
                                               Slogan = s.Slogan,
                                               CategoryId = c.Id,
                                               ProductCategory = c.Name,
                                               CreateAt = p.CreateAt,
                                               UpdateAt = p.UpdateAt
                                           }
                                     ).OrderByDescending(t => t.CreateAt).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return listproductdetail;
        }

        public async Task<ProductDetailsModel> GetById(string proId)
        {
            var product = await (from p in context.Products
                                 join s in context.Stores on p.StoreId equals s.Id
                                 join c in context.Categories on p.CategoryId equals c.Id
                                 where p.Id == proId
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
                                     MinimumDeIn = p.MinimumDeIn,
                                     Rate = p.Rate,
                                     StoreId = s.Id,
                                     StoreName = s.Name,
                                     StoreImage = s.Image,
                                     Slogan = s.Slogan,
                                     CategoryId = c.Id,
                                     ProductCategory = c.Name,
                                     CreateAt = p.CreateAt,
                                     UpdateAt = p.UpdateAt
                                 }).FirstOrDefaultAsync();

            return product;
        }
        public async Task<ProductModel> CreatNewProduct(ProductModel pro)
        {
            string fileImg = "ImagesProducts";
            string time = await GetTime();
            context.Products.Add(
                new Product {
                    Id = Guid.NewGuid().ToString(), 
                    Name = pro.Name,
                    Image = await _fileService.UploadFile(fileImg,pro.Image),
                    Unit = pro.Unit,
                    PricePerPack= pro.PricePerPack,
                    PackDescription= pro.PackDescription,
                    PackNetWeight= pro.PackNetWeight,
                    MaximumQuantity= pro.MaximumQuantity,
                    MinimumQuantity = pro.MinimumQuantity,
                    MinimumDeIn = pro.MinimumDeIn,
                    StoreId = pro.StoreId,
                    CategoryId = pro.CategoryId,
                    Rate = pro.Rate,
                    Description = pro.Description,
                    CreateAt = time
                    });
            await context.SaveChangesAsync();
            return pro;
        }
        public async Task<Object> UpdateProductById(string proId, ProductDto product)
        {
           
            string fileImg = "ImagesProducts";
            string time = await GetTime();
            var pro = await context.Products.FindAsync(proId);
            var store = context.Stores.FirstOrDefault(s => s.Id == product.StoreId);
            var category = context.Categories.FirstOrDefault(c => c.Id == product.CategoryId);
            pro.Name = product.Name;
            pro.Image = await _fileService.UploadFile(fileImg, product.Image);
            pro.Unit = product.Unit;
            pro.PricePerPack = product.PricePerPack;
            pro.PackNetWeight = product.PackNetWeight;
            pro.PackDescription = product.PackDescription;
            pro.MaximumQuantity = product.MaximumQuantity;
            pro.MinimumQuantity = product.MinimumQuantity;
            pro.Description = product.Description;
            pro.Rate = product.Rate;
            pro.StoreId = product.StoreId;
            pro.CategoryId = product.CategoryId;
            pro.UpdateAt = time;

            var menu = new Menu();
            var listProInMenu = await context.ProductInMenus.Where(pm => pm.ProductId == proId).ToListAsync();
            var listCateInMenu = await context.CategoryInMenus.Where(cm => cm.CategoryId == product.CategoryId).ToListAsync();
            if (listProInMenu.Any())
            {
                if (listCateInMenu.Any()) throw new Exception("Category currently in the Menu");
                //context.CategoryInMenus.RemoveRange(listCateInMenu);
            }
            context.Entry(pro).State = EntityState.Modified; 
            try
                {
                    await context.SaveChangesAsync();
                }
                catch
                {
                    throw;
                } 
            return product;
            }
        public async Task<Object> DeleteProductById(string id)  
        {
            var product = await context.Products.FindAsync(id);
            if (product == null)
            {
                return null;
            }
            context.Products.Remove(product);
            await context.SaveChangesAsync();

            return product;
        }
        public async Task<string> GetTime()
        {
            DateTime utcDateTime = DateTime.UtcNow;
            string vnTimeZoneKey = "SE Asia Standard Time";
            TimeZoneInfo vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(vnTimeZoneKey);
            string time = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, vnTimeZone).ToString("yyyy/MM/dd HH:mm");
            return time;
        }
    }
}
