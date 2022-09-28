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
                }).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return listCate;
        }
        public async Task<CategoryModel> CreateCategory(CategoryModel category)
        {
            _context.Categories.Add(new Category { Id = category.Id, Name = category.Name });
            await _context.SaveChangesAsync();
            return category;

        }

        public async Task<Object> DeleteCateInMenuById(string CateInMenuId)
        {
            var CateMenu = await _context.CategoryInMenus.FindAsync(CateInMenuId);
            _context.CategoryInMenus.Remove(CateMenu);
            await _context.SaveChangesAsync();

            return CateInMenuId;

        }
        public async Task<Object> UpdateCategoryById(string categoryId, CategoryModel category)
        {
            if (categoryId == null)
            {
                return null;
            }
            var result = await _context.Categories.FindAsync(categoryId);
            result.Id = category.Id;
            result.Name = category.Name;

            _context.Entry(result).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
            return category;
        }
        public async Task<List<CategoryModel>> GetListCategoryByMenuId(string id, int page, int pageSize)
        {
            var listCategories = await (from c in _context.Categories
                                      join cm in _context.CategoryInMenus on c.Id equals cm.CategoryId
                                      join menu in _context.Menus on cm.MenuId equals menu.Id
                                      where menu.Id == id
                                        select new CategoryModel
                                      {
                                          Id = c.Id,
                                          Name = c.Name,
                                          Image = c.Image
                                      }).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return listCategories;
        }
    }
}
