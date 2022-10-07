using DeliveryVHGP_WebApi.IRepositories;
using DeliveryVHGP_WebApi.Models;
using DeliveryVHGP_WebApi.ViewModels;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.EntityFrameworkCore;

namespace DeliveryVHGP_WebApi.Repositories
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly DeliveryVHGP_DBContext _context;
        private static string apiKey = "AIzaSyAauR7Lp1qtRLPIOkONgrLyPYLrdjN_qKw";
        private static string apibucket = "lucky-science-341916.appspot.com";
        private static string authenEmail = "adminstore2@gmail.com";
        private static string authenPassword = "store123456";
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
        public async Task<Object> PostFireBase(IFormFile file)
        {
            var fileUpload = file;
            FileStream fs = null;
            if (fileUpload.Length > 0)
            {
                {
                    string folderName = "ImagesCategories";
                    string path = Path.Combine($"Image/{folderName}");
                    if (Directory.Exists(path))
                    {
                        using (fs = new FileStream(Path.Combine(path, fileUpload.FileName), FileMode.Create))
                        {
                            await fileUpload.CopyToAsync(fs);
                        }
                        fs = new FileStream(Path.Combine(path, fileUpload.FileName), FileMode.Open);
                    }
                    else
                    {
                        Directory.CreateDirectory(path);
                    }

                }
                var authen = new FirebaseAuthProvider(new FirebaseConfig(apiKey));
                var a = await authen.SignInWithEmailAndPasswordAsync(authenEmail, authenPassword);
                var cancel = new CancellationTokenSource();
                var upload = new FirebaseStorage(
                    apibucket,
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                        ThrowOnCancel = true
                    }
                    ).Child("ImageCategories").Child(fileUpload.FileName).PutAsync(fs, cancel.Token);
                try
                {
                    string Link = await upload;
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return file;
        }
    }
}
