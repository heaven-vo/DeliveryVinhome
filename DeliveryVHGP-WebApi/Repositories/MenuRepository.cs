using DeliveryVHGP_WebApi.IRepositories;
using DeliveryVHGP_WebApi.Models;
using DeliveryVHGP_WebApi.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DeliveryVHGP_WebApi.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        private readonly DeliveryVHGP_DBContext context;

        public MenuRepository(DeliveryVHGP_DBContext context)
        {
            this.context = context;
        }
        public async Task<List<MenuView>> getListMenuNow()
        {
            DateTime utcDateTime = DateTime.UtcNow;
            string vnTimeZoneKey = "SE Asia Standard Time";
            TimeZoneInfo vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(vnTimeZoneKey);
            string time = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, vnTimeZone).ToString("HH.mm");
            var time2 = Double.Parse(time);
            var listMenu = await context.Menus.Where(x => x.StartHour <= time2).Where(x => x.EndHour > time2).OrderBy(x => x.ModeId).Select(x => new MenuView
            {
                Id = x.Id,
                Image = x.Image,
                Name = x.Name
            }).ToListAsync();
            foreach(var menu in listMenu)
            {
                menu.ListProducts = await getListProductInMenu(menu.Id, 1, 5);
            }
            return listMenu;
        }
        public async Task<List<ProductViewInList>> getListProductInMenu(string id, int page, int pageSize)
        {
            var listProduct = await(from menu in context.Menus
                                    join pm in context.ProductInMenus on menu.Id equals pm.MenuId
                                    join product in context.Products on pm.ProductId equals product.Id
                                    join store in context.Stores on product.StoreId equals store.Id
                                    where menu.Id == id
                                    select new ProductViewInList
                                    {
                                        Id = product.Id,
                                        Image = product.Image,
                                        Name = product.Name,
                                        PricePerPack = pm.Price,
                                        PackDes = product.PackDescription,
                                        StoreName = store.Name
                                    }).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return listProduct;
        }
    }
}
