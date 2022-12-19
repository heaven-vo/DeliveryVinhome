using DeliveryVHGP.Core.Data;
using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Core.Interface.IRepositories;
using DeliveryVHGP.Core.Models;
using DeliveryVHGP.Infrastructure.Repositories.Common;
using DeliveryVHGP.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace DeliveryVHGP.WebApi.Repositories
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        private readonly IFileService _fileService;
        private readonly ITimeStageService _timeStageService;
        public ProductRepository(ITimeStageService timeStageService, IFileService fileService, DeliveryVHGP_DBContext context) : base(context)
        {
            _fileService = fileService;
            _timeStageService = timeStageService;
        }
        public async Task<IEnumerable<ProductDetailsModel>> GetListProduct(string menuId, int pageIndex, int pageSize)
        {
            var listproductdetail = await (from p in context.Products
                                           join s in context.Stores on p.StoreId equals s.Id
                                           join c in context.Categories on p.CategoryId equals c.Id
                                           join pm in context.ProductInMenus on p.Id equals pm.ProductId
                                           join m in context.Menus on pm.MenuId equals m.Id
                                           where m.Id == menuId
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
                                               UpdateAt = p.UpdateAt,
                                               Status = pm.Status
                                           }
                                     ).OrderByDescending(t => t.CreateAt).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return listproductdetail;
        }
        public async Task<IEnumerable<ProductDetailsModel>> GetAll(string storeId, int pageIndex, int pageSize)
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
            string time = await _timeStageService.GetTime();
            context.Products.Add(
                new Product
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = pro.Name,
                    Image = await _fileService.UploadFile(fileImg, pro.Image),
                    Unit = pro.Unit,
                    PricePerPack = pro.PricePerPack,
                    PackDescription = pro.PackDescription,
                    PackNetWeight = pro.PackNetWeight,
                    MaximumQuantity = pro.MaximumQuantity,
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
            string time = await _timeStageService.GetTime();
            var pro = await context.Products.FindAsync(proId);
            var store = context.Stores.FirstOrDefault(s => s.Id == product.StoreId);
            var category = context.Categories.FirstOrDefault(c => c.Id == pro.CategoryId);
            var p = new Product();
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

            var listProInMenu = await context.ProductInMenus.Where(pm => pm.ProductId == proId).ToListAsync();
            var listCateInMenu = await context.CategoryInMenus.Where(cm => cm.CategoryId == product.CategoryId).ToListAsync();
            if (listProInMenu.Any())
            {
                if (product.CategoryId != category.Id)
                    if (listCateInMenu.Any()) throw new Exception("Category currently in the Menu");
            }

            //context.CategoryInMenus.RemoveRange(listCateInMenu);

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
    }
}
