using DeliveryVHGP_WebApi.IRepositories;
using DeliveryVHGP_WebApi.Models;
using DeliveryVHGP_WebApi.ViewModels;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.EntityFrameworkCore;

namespace DeliveryVHGP_WebApi.Repositories
{
    public class BrandRepository : IBrandRepository

    {
        private readonly DeliveryVHGP_DBContext _context;
        private static string apiKey = "AIzaSyAauR7Lp1qtRLPIOkONgrLyPYLrdjN_qKw";
        private static string apibucket = "lucky-science-341916.appspot.com";
        private static string authenEmail = "adminstore2@gmail.com";
        private static string authenPassword = "store123456";
        public BrandRepository (DeliveryVHGP_DBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BrandModels>> GetAll(int pageIndex, int pageSize)
        {
            var listbrand = await _context.Brands.
                Select(x => new BrandModels
            {
                Id = x.Id,
                Name = x.Name,
                Image = x.Image
            }).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return listbrand;
        }

        public async Task<BrandModels>GetById(string id)
        {
            var brand = await _context.Brands.Where(x => x.Id == id).Select(x => new BrandModels
            {
                Id=x.Id,
                Name=x.Name,
                Image=x.Image
            }).FirstOrDefaultAsync();
            return brand;
        }
        public async Task<BrandModels> CreateBrand(BrandModels brand)
        {
            _context.Brands.Add(new Brand { Id = brand.Id, Name = brand.Name, Image = brand.Image });
            await _context.SaveChangesAsync();
            return brand;

        }

        public async Task<Object> DeleteById(string brandId)
        {
            var brand = await _context.Brands.FindAsync(brandId);
            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();

            return brand;

        }

        public async Task<Object> UpdateBrandById(string brandId, BrandModels brand)
        {
            if (brandId == null)
            {
                return null;
            }
            var result = await _context.Brands.FindAsync(brandId);
            result.Id = brand.Id;
            result.Name = brand.Name;
            result.Image = brand.Image;

            _context.Entry(result).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
            return brand;
        }
        public async Task<Object> PostFireBase(IFormFile file)
        {
            var fileUpload = file;
            FileStream fs = null;
            if (fileUpload.Length > 0)
            {
                {
                    string folderName = "ImagesBrands";
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
                    ).Child("ImageBrand").Child(fileUpload.FileName).PutAsync(fs, cancel.Token);
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
