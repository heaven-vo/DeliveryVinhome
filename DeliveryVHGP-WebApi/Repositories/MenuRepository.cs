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

        public async Task<List<MenuView>> GetListMenuNow()
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
            foreach (var menu in listMenu)
            {
                menu.ListProducts = await GetListProductInMenu(menu.Id, 1, 5);
            }
            return listMenu;
        }
        public async Task<List<MenuView>> GetListMenuByMode(string modeId)
        {
            DateTime utcDateTime = DateTime.UtcNow;
            string vnTimeZoneKey = "SE Asia Standard Time";
            TimeZoneInfo vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(vnTimeZoneKey);
            string time = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, vnTimeZone).ToString("HH.mm");
            var time2 = Double.Parse(time);
            var listMenu = await context.Menus.Where(x => x.ModeId == modeId).Where(x => x.StartHour <= time2).Where(x => x.EndHour > time2).OrderBy(x => x.ModeId).Select(x => new MenuView
            {
                Id = x.Id,
                Image = x.Image,
                Name = x.Name
            }).ToListAsync();
            foreach (var menu in listMenu)
            {
                menu.ListProducts = await GetListProductInMenu(menu.Id, 1, 5);
            }
            return listMenu;
        }
        public async Task<List<ProductViewInList>> GetListProductInMenu(string id, int page, int pageSize)
        {
            var listProduct = await (from menu in context.Menus
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
        public async Task<MenuDto> CreatNewMenu(MenuDto menu)
        {
            var id = Guid.NewGuid().ToString();
            var newMenu = new Menu
            {
                Id = id,
                Name = menu.Name,
                Image = menu.Image,
                StartHour = menu.StartHour,
                EndHour = menu.EndHour,
                ModeId = menu.ModeId
            };
            try
            {
                context.Add(newMenu);
                await context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
            return menu;
        }

        public async Task<MenuDto> UpdateMenu(string menuId, MenuDto menu)
        {
            if (menuId == null || menu.Id == null || menuId != menu.Id)
            {
                return null;
            }
            var menuUpdate = new Menu
            {
                Id = menuId,
                Name = menu.Name,
                Image = menu.Image,
                StartHour = menu.StartHour,
                EndHour = menu.EndHour,
                ModeId = menu.ModeId
            };
            context.Entry(menuUpdate).State = EntityState.Modified;
            try
            {           
                await context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
            return menu;
            
        }
    }
}
