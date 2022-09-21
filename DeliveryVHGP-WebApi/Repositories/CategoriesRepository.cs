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

        public async Task<CategoryModel> GetById(string Id)
        {
            var categoryy = await _context.Categories.Where(x => x.Id == Id).Select(x => new CategoryModel
            {
                Id = x.Id,
                Name = x.Name,
            }).FirstOrDefaultAsync();
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
        public async Task<CategoryModel> CreateCategory(CategoryModel category)
        {
            _context.Categories.Add(new Category { Id = category.Id, Name = category.Name });
            await _context.SaveChangesAsync();
            return category;

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
    }
}
