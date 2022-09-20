using DeliveryVHGP_WebApi.IRepositories;
using DeliveryVHGP_WebApi.Models;
using DeliveryVHGP_WebApi.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DeliveryVHGP_WebApi.Repositories
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly DeliveryVHGP_DBContext _context;

        public CategoriesRepository(DeliveryVHGP_DBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryModel>> GetAll(int pageIndex, int pageSize)
        {
            var listCate = await _context.Categories.
                Select(x => new CategoryModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Image = x.Image
                }).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            foreach (CategoryModel category in listCate)
            {
                category.ListCateInMenus = await GetCategoryByMenuId(category.Id);
                category.ListProducts = await GetCategoryByProduct(category.Id);
            }
                return listCate;
        }

        public async Task<CategoryModel> GetById(string Id)
        {
            var categoryy = await _context.Categories.Where(x => x.Id == Id).Select(x => new CategoryModel
            {
                Id = x.Id,
                Name = x.Name,
                Image = x.Image
            }).FirstOrDefaultAsync();
            if (categoryy != null)
            {
                categoryy.ListCateInMenus = await GetCategoryByMenuId(categoryy.Id);
                categoryy.ListProducts = await GetCategoryByProduct(categoryy.Id);
            }
                return categoryy;
        }
        public async Task<List<string>> GetCategoryByMenuId(string menuId)
        {
            List<string> ListCategoryInMenus = await (from c in _context.Categories
                                                      join cm in _context.CategoryInMenus on c.Id equals cm.CategoryId
                                                      join m in _context.Menus on cm.MenuId equals m.Id
                                            where  c.Id == menuId
                                            select m.Name
                              ).ToListAsync();
            return ListCategoryInMenus;
        }
        public async Task<List<string>> GetCategoryByProduct(string productId)
        {
            List<string> ListProduct = await (from c in _context.Categories
                                                      join p in _context.Products on c.Id equals p.CategoryId
                                                      where c.Id == productId
                                                      select p.Name
                              ).ToListAsync();
            return ListProduct;
        }

    }
}
