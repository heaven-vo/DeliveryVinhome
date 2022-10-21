using DeliveryVHGP.Core.Interface.IRepositories;
using DeliveryVHGP.Core.Data;
using DeliveryVHGP.Core.Models;
using Microsoft.EntityFrameworkCore;
using DeliveryVHGP.Infrastructure.Services;
using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Infrastructure.Repositories.Common;

namespace DeliveryVHGP.WebApi.Repositories
{
    public class CategoriesRepository : RepositoryBase<Category>, ICategoriesRepository
    {
        private readonly IFileService _fileService;
        public CategoriesRepository(IFileService fileService, DeliveryVHGP_DBContext context): base(context)
        {
            _fileService = fileService;
        }

        public async Task<IEnumerable<CategoryModel>> GetAll(int pageIndex, int pageSize)
        {
            var listCate = await context.Categories.
                Select(x => new CategoryModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Image = x.Image,
                    CreateAt = x.CreateAt,
                    UpdateAt = x.UpdateAt,
                }).OrderByDescending(t => t.CreateAt).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return listCate;
        }
        public async Task<Object> GetCategoryById(string cateId)
        {
            var cate = await context.Categories.Where(x => x.Id == cateId)
                                     .Select(x => new CategoryModel
                              {
                                   Id = x.Id,
                                   Name = x.Name,
                                   Image = x.Image,
                                   CreateAt = x.CreateAt,
                                   UpdateAt = x.UpdateAt,
                               }).FirstOrDefaultAsync();
            return cate;
        }
        public async Task<IEnumerable<CategoryModel>> GetListCategoryByName(string cateName, int pageIndex, int pageSize)
        {
            //cateName = Regex.Replace(cateName, @"[^\sa-zA-Z]", string.Empty).Trim();
            //cateName = await ConvertString(cateName);
            //string param1 = new SqlParameter("@Name", cateName);
            var listCate = await (from cate in context.Categories
                                  .Where( cate => cate.Name.Contains(cateName))
                                   select new CategoryModel()
                                   {
                                       Id = cate.Id,
                                       Name = cate.Name,
                                       Image = cate.Image,
                                       CreateAt = cate.CreateAt,
                                       UpdateAt = cate.UpdateAt
                                   }).OrderByDescending(t => t.CreateAt).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return listCate;
        }
        public async Task<CategoryDto> CreateCategory(CategoryDto category)
        {
            string fileImg = "ImagesCategorys"; 
            string time = await GetTime();
            context.Categories.Add(
                new Category {
                Id = Guid.NewGuid().ToString(),
                Name = category.Name,
                Image = await _fileService.UploadFile(fileImg , category.Image),
                CreateAt = time
            });
            await context.SaveChangesAsync();
            return category;
        }

        public async Task<Object> DeleteCateInMenuById(string CateInMenuId)
        {
            var CateMenu = await context.CategoryInMenus.FindAsync(CateInMenuId);
            context.CategoryInMenus.Remove(CateMenu);
            await context.SaveChangesAsync();

            return CateInMenuId;

        }
        public async Task<Object> UpdateCategoryById(string categoryId, CategoryDto category, Boolean imgUpdate)
        {
            string fileImg = "ImagesCategorys";
            string time = await GetTime();
            var result = await context.Categories.FindAsync(categoryId);
            result.Name = category.Name;
            if (imgUpdate == true)
            {
                result.Image = await _fileService.UploadFile(fileImg, category.Image);
            }
            result.UpdateAt = time;
            context.Entry(result).State = EntityState.Modified;
            try
            {
                await context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
            return category;
        }
        public async Task<List<CategoryModel>> GetListCategoryByMenuId(string id, int page, int pageSize)
        {
            var listCategories = await (from c in context.Categories
                                      join cm in context.CategoryInMenus on c.Id equals cm.CategoryId
                                      join menu in context.Menus on cm.MenuId equals menu.Id
                                      where menu.Id == id
                                        select new CategoryModel
                                        {
                                          Id = c.Id,
                                          Name = c.Name,
                                          Image = c.Image,
                                          CreateAt = c.CreateAt,
                                          UpdateAt = c.UpdateAt
                                      }).OrderByDescending(t => t.CreateAt).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return listCategories;
        }
        public async Task<string> GetTime()
        {
            DateTime utcDateTime = DateTime.UtcNow;
            string vnTimeZoneKey = "SE Asia Standard Time";
            TimeZoneInfo vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(vnTimeZoneKey);
            string time = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, vnTimeZone).ToString("yyyy/MM/dd HH:mm");
            return time;
        }
        public async Task<string> ConvertString(string stringInput)
        {
            stringInput = stringInput.ToUpper();
            string convert = "ĂÂÀẰẦÁẮẤẢẲẨÃẴẪẠẶẬỄẼỂẺÉÊÈỀẾẸỆÔÒỒƠỜÓỐỚỎỔỞÕỖỠỌỘỢƯÚÙỨỪỦỬŨỮỤỰÌÍỈĨỊỲÝỶỸỴĐăâàằầáắấảẳẩãẵẫạặậễẽểẻéêèềếẹệôòồơờóốớỏổởõỗỡọộợưúùứừủửũữụựìíỉĩịỳýỷỹỵđ";
string To = "AAAAAAAAAAAAAAAAAEEEEEEEEEEEOOOOOOOOOOOOOOOOOUUUUUUUUUUUIIIIIYYYYYDaaaaaaaaaaaaaaaaaeeeeeeeeeeeooooooooooooooooouuuuuuuuuuuiiiiiyyyyyd";
for (int i = 0; i < To.Length; i++)
            {
                stringInput = stringInput.Replace(convert[i], To[i]);
            }
            return stringInput;
        }
    }
}
