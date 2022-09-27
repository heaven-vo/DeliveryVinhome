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
        public async Task<MenuDto> GetMenuDetail(string menuId)
        {
            var menu = await context.Menus.FindAsync(menuId);
            if (menu == null)
            {
                throw new Exception("Not Found");
            }
            List<string> cateId = await context.CategoryInMenus.Where(x => x.MenuId == menuId).Select(x => x.CategoryId).ToListAsync();

            MenuDto menuDto = new MenuDto
            {
                Name = menu.Name,
                Image = menu.Image,
                DayFilter = menu.DayFilter,
                StartDate = menu.StartDate,
                EndDate = menu.EndDate,
                HourFilter = menu.HourFilter,
                StartHour = menu.StartHour,
                EndHour = menu.EndHour,
                listCategory = cateId,
                ModeId = menu.ModeId
            };
            return menuDto;
        }

        //Get a menu by mode id and group by store (in customer web) 
        public async Task<MenuView> GetMenuByModeAndGroupByStore(string modeId, int page, int pageSize)
        {
            double time = await GetTime();
            var menuView = await context.Menus.Where(x => x.ModeId == modeId).Where(x => x.StartHour <= time).Where(x => x.EndHour > time).Select(x => new MenuView
            {
                Id = x.Id,
                Name = x.Name,
                Image = x.Image,
                StartTime = x.StartHour,
                EndTime = x.EndHour
            }).FirstOrDefaultAsync();
            if (menuView.Id == null) throw new Exception("Not found menu");

            var listStore = await (from menu in context.Menus
                                   join sm in context.StoreInMenus on menu.Id equals sm.MenuId
                                   join store in context.Stores on sm.StoreId equals store.Id
                                   where menu.Id == menuView.Id
                                   select new CategoryStoreInMenu
                                   {
                                       Id = store.Id,
                                       Name = store.Name,
                                       Image = store.Image

                                   }).ToListAsync();
            foreach (var store in listStore)
            {
                var listProduct = await GetListProductInMenuByStoreId(store.Id, menuView.Id, page, pageSize);
                store.ListProducts = listProduct;
            }
            menuView.ListCategoryStoreInMenus = listStore;
            return menuView;
        }

        //Get a menu by mode id and group by category (in customer web) 
        public async Task<MenuView> GetMenuByModeAndGroupByCategory(string modeId, int page, int pageSize)
        {
            double time = await GetTime();
            var menuView = await context.Menus.Where(x => x.ModeId == modeId).Where(x => x.StartHour <= time).Where(x => x.EndHour > time).Select(x => new MenuView
            {
                Id = x.Id,
                Name = x.Name,
                Image = x.Image,
                StartTime = x.StartHour,
                EndTime = x.EndHour
            }).FirstOrDefaultAsync();
            if (menuView == null) throw new Exception("Not found menu");

            var listCategory = await (from menu in context.Menus
                                  join cm in context.CategoryInMenus on menu.Id equals cm.MenuId
                                  join category in context.Categories on cm.CategoryId equals category.Id
                                  where menu.Id == menu.Id
                                  select new CategoryStoreInMenu
                                  {
                                      Id = category.Id,
                                      Name = category.Name,
                                      Image = category.Image
                                  }).ToListAsync();
            foreach (var category in listCategory)
            {
                var listProduct = await GetListProductInMenuByCategoryId(category.Id, menuView.Id, page, pageSize);
                if (listProduct != null)
                    category.ListProducts = listProduct;
            }
            listCategory = listCategory.Where(x => x.ListProducts != null).ToList();
            menuView.ListCategoryStoreInMenus = listCategory;
            return menuView;
        }

        // Get a menu in store include list product group by category(in store web)
        public async Task<List<CategoryStoreInMenu>> GetMenuByMenuIdAndStoreIdAndGroupByCategory(string menuId, string storeId, int page, int pageSize)
        {
            //Get list cate by menu id
            var listCategory = await (from menu in context.Menus
                                  join cm in context.CategoryInMenus on menu.Id equals cm.MenuId
                                  join category in context.Categories on cm.CategoryId equals category.Id
                                  where menu.Id == menuId
                                  select new CategoryStoreInMenu
                                  {
                                      Id = category.Id,
                                      Name = category.Name,
                                      Image = category.Image
                                  }).ToListAsync();
            foreach (var category in listCategory)
            {
                var listProduct = await GetListProductInMenuByCategoryIdAndStoreId(storeId, category.Id, menuId, page, pageSize);
                if (listProduct != null)
                    category.ListProducts = listProduct;
            }
            listCategory = listCategory.Where(x => x.ListProducts != null).ToList();
            return listCategory;
        }
        public async Task<List<ProductViewInList>> GetListProductInMenuByStoreId(string storeId, string menuId, int page, int pageSize)
        {
            var listProducts = await (from product in context.Products
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
        // Get products in menu of store filter by category (load list product in menu for a store)
        public async Task<List<ProductViewInList>> GetListProductInMenuByCategoryIdAndStoreId(string storeId, string categoryId, string menuId, int page, int pageSize)
        {
            var listProducts = await (from product in context.Products
                                      join store in context.Stores on product.StoreId equals store.Id
                                      join category in context.Categories on product.CategoryId equals category.Id
                                      join pm in context.ProductInMenus on product.Id equals pm.ProductId
                                      join menu in context.Menus on pm.MenuId equals menu.Id
                                      where store.Id == storeId && category.Id == categoryId && menu.Id == menuId
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

        //Get list product in store not in menu (when add product to menu in store web)
        public async Task<List<ProductViewInList>> GetListProductNotInMenuByCategoryIdAndStoreId(string storeId, string menuId, int page, int pageSize)
        {
            var listProduct = await (from product in context.Products
                                     join store in context.Stores on product.StoreId equals store.Id
                                     join pm in context.ProductInMenus on product.Id equals pm.ProductId into nx
                                     from x in nx.DefaultIfEmpty()
                                     where store.Id == storeId && x.MenuId != menuId
                                     select new ProductViewInList
                                     {
                                         Id = product.Id,
                                         Image = product.Image,
                                         Name = product.Name,
                                         PricePerPack = product.PricePerPack,
                                         PackDes = product.PackDescription,
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
            foreach (var category in menu.listCategory)
            {
                var cmId = Guid.NewGuid().ToString();
                CategoryInMenu cate = new CategoryInMenu { Id = cmId, CategoryId = category, MenuId = id };
                context.CategoryInMenus.Add(cate);
            }
            try
            {
                context.Menus.Add(newMenu);
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
            var menuUpdate = new Menu
            {
                Id = menuId,
                Name = menu.Name,
                Image = menu.Image,
                StartHour = menu.StartHour,
                EndHour = menu.EndHour,
                ModeId = menu.ModeId
            };
            //remove old category in menu
            var listCateInMenu = await context.CategoryInMenus.Where(x => x.MenuId == menuId).ToListAsync();
            context.CategoryInMenus.RemoveRange(listCateInMenu);
            //add new category in menu
            foreach (var category in menu.listCategory)
            {
                var cmId = Guid.NewGuid().ToString();
                CategoryInMenu cate = new CategoryInMenu { Id = cmId, CategoryId = category, MenuId = menuId };
                context.CategoryInMenus.Add(cate);
            }

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
