using DeliveryVHGP_WebApi.IRepositories;
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
        public async Task<List<ProductViewInList>> getListProductInStore(string storeId, int page, int pageSize) 
        {
            double time = await getTime();
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

        public async Task<List<ProductViewInList>> getListProductInCategory(string categoryId, int page, int pageSize)
        {
            double time = await getTime();
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
        public async Task<double> getTime()
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
