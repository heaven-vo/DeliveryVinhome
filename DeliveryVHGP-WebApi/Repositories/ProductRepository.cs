﻿using DeliveryVHGP_WebApi.IRepositories;
using DeliveryVHGP_WebApi.Models;
using DeliveryVHGP_WebApi.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DeliveryVHGP_WebApi.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DeliveryVHGP_DBContext context;

        public ProductRepository(DeliveryVHGP_DBContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<ProductDetailsModel>> GetAll(int pageIndex, int pageSize)
        {
            var listproductdetail = await (from p in context.Products
                                           join s in context.Stores on p.StoreId equals s.Id
                                           join c in context.Categories on p.CategoryId equals c.Id
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
            var pro = await context.Products.FindAsync(proId);
            var store = context.Stores.Where(x => x.Name == product.StoreName).Where(x => x.Image == product.StoreImage).Select(x => x.Id).FirstOrDefault();
            var category = context.Categories.Where(c => c.Name == product.ProductCategory).Select(c => c.Id).FirstOrDefault();
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
        // du GetListProductInStore va GetListProductInCategory
        public async Task<List<ProductViewInList>> GetListProductInStore(string storeId, int page, int pageSize) 
        {
            double time = await GetTime();
            var listProducts = await ( from product in context.Products
                                       join store in context.Stores on product.StoreId equals store.Id
                                       join pm in context.ProductInMenus on product.Id equals pm.ProductId
                                       join menu in context.Menus on pm.MenuId equals menu.Id
                                       where store.Id == storeId && menu.StartHour <= time && menu.EndHour > time
                                       select new ProductViewInList
                                       {
                                           Id = product.Id,
                                           Image = product.Image,
                                           Name = product.Name,
                                           PricePerPack = pm.Price,
                                           PackDes = product.PackDescription,
                                           StoreName = store.Name
                                       }).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return listProducts;
        }

        public async Task<List<ProductViewInList>> GetListProductInCategory(string categoryId, int page, int pageSize)
        {
            double time = await GetTime();
            var listProducts = await (from product in context.Products
                                      join store in context.Stores on product.StoreId equals store.Id
                                      join category in context.Categories on product.CategoryId equals category.Id
                                      join pm in context.ProductInMenus on product.Id equals pm.ProductId
                                      join menu in context.Menus on pm.MenuId equals menu.Id
                                      where category.Id == categoryId && menu.StartHour <= time && menu.EndHour > time
                                      select new ProductViewInList
                                      {
                                          Id = product.Id,
                                          Image = product.Image,
                                          Name = product.Name,
                                          PricePerPack = pm.Price,
                                          PackDes = product.PackDescription,
                                          StoreName = store.Name
                                      }).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return listProducts;
        }
        public async Task<double> GetTime()
        {
            DateTime utcDateTime = DateTime.UtcNow;
            string vnTimeZoneKey = "SE Asia Standard Time";
            TimeZoneInfo vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(vnTimeZoneKey);
            string time = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, vnTimeZone).ToString("HH.mm");
            var time2 = Double.Parse(time);
            return time2;
        }
    }
}
