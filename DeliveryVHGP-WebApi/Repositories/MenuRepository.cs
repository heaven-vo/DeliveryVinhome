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

        public async Task<List<MenuView>> GetListMenuName()
        {
            double time = await GetTime();
            var listMenu = await context.Menus.Where(x => x.StartHour <= time).Where(x => x.EndHour > time).OrderBy(x => x.ModeId).Select(x => new MenuView
            {
                Id = x.Id,
                Image = x.Image,
                Name = x.Name
            }).ToListAsync();
            return listMenu;
        }
        public async Task<List<MenuView>> GetListMenuNow()
        {
            double time = await GetTime();
            var listMenu = await context.Menus.Where(x => x.StartHour <= time).Where(x => x.EndHour > time).OrderBy(x => x.ModeId).Select(x => new MenuView
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
            double time = await GetTime();
            var listMenu = await context.Menus.Where(x => x.ModeId == modeId).Where(x => x.StartHour <= time).Where(x => x.EndHour > time).Select(x => new MenuView
            {
                Id = x.Id,
                Image = x.Image,
                Name = x.Name
            }).ToListAsync();
            foreach (var menu in listMenu)
            {
                menu.ListProducts = await GetListProductInMenu(menu.Id, 1, 12);
            }
            return listMenu;
        }
        public async Task<List<MenuView>> GetListMenuByStoreId(string storeId)
        {
            double time = await GetTime();
            var listMenu = await ( from menu in context.Menus
                                   join sm in context.StoreInMenus on menu.Id equals sm.MenuId
                                   join store in context.Stores on sm.StoreId equals store.Id
                                   where store.Id == storeId && menu.StartHour <= time && menu.EndHour > time
                                   select new MenuView { Id = menu.Id, Image = menu.Image, Name = menu.Name}
                ).ToListAsync();
            foreach (var menu in listMenu)
            {
                menu.ListProducts = await GetListProductInMenuByStoreId(storeId, menu.Id, 1, 12);
            }
            return listMenu;
        }

        public async Task<List<MenuView>> GetListMenuByCategoryId(string categoryId)
        {
            double time = await GetTime();
            var listMenu = await (from menu in context.Menus
                                  join cm in context.CategoryInMenus on menu.Id equals cm.MenuId
                                  join category in context.Categories on cm.CategoryId equals category.Id
                                  where category.Id == categoryId && menu.StartHour <= time && menu.EndHour > time
                                  select new MenuView { Id = menu.Id, Image = menu.Image, Name = menu.Name }
                ).ToListAsync();
            foreach (var menu in listMenu)
            {
                menu.ListProducts = await GetListProductInMenuByCategoryId(categoryId, menu.Id, 1, 12);
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
        public async Task<List<ProductViewInList>> GetListProductInMenuByStoreId(string storeId, string menuId, int page, int pageSize)
        {
            double time = await GetTime();
            var listProducts = await(from product in context.Products
                                     join store in context.Stores on product.StoreId equals store.Id
                                     join pm in context.ProductInMenus on product.Id equals pm.ProductId
                                     join menu in context.Menus on pm.MenuId equals menu.Id
                                     where store.Id == storeId && menu.Id == menuId
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

        public async Task<List<ProductViewInList>> GetListProductInMenuByCategoryId(string categoryId, string menuId, int page, int pageSize)
        {
            double time = await GetTime();
            var listProducts = await (from product in context.Products
                                      join store in context.Stores on product.StoreId equals store.Id
                                      join category in context.Categories on product.CategoryId equals category.Id
                                      join pm in context.ProductInMenus on product.Id equals pm.ProductId
                                      join menu in context.Menus on pm.MenuId equals menu.Id
                                      where category.Id == categoryId && menu.Id == menuId
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
