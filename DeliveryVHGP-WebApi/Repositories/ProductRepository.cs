using DeliveryVHGP_WebApi.IRepositories;
using DeliveryVHGP_WebApi.Models;
using DeliveryVHGP_WebApi.ViewModels;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.EntityFrameworkCore;

namespace DeliveryVHGP_WebApi.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IFileService _fileService;
        private readonly DeliveryVHGP_DBContext context;
        private static string apiKey = "AIzaSyAauR7Lp1qtRLPIOkONgrLyPYLrdjN_qKw";
        private static string apibucket = "lucky-science-341916.appspot.com";
        private static string authenEmail = "adminstore2@gmail.com";
        private static string authenPassword = "store123456";
        public ProductRepository(IFileService fileService, DeliveryVHGP_DBContext context)
        {
            this.context = context;
            _fileService = fileService;
        }
        public async Task<IEnumerable<ProductDetailsModel>> GetAll(string storeId,int pageIndex, int pageSize)
        {
            var listproductdetail = await (from p in context.Products
                                           join s in context.Stores on p.StoreId equals s.Id
                                           join c in context.Categories on p.CategoryId equals c.Id
                                           where s.Id == storeId
                                           select new ProductDetailsModel()
                                           {
                                               Id = p.Id,
                                               Name = p.Name,
                                               Image = p.Image,
                                               Unit = p.Unit,
                                               PricePerPack = p.PricePerPack,
                                               PackNetWeight = p.PackNetWeight,
                                               PackDescription = p.PackDescription,
                                               MaximumQuantity = p.MaximumQuantity,
                                               MinimumQuantity = p.MinimumQuantity,
                                               Description = p.Description,
                                               MinimumDeIn = p.MinimumDeIn,
                                               Rate = p.Rate,
                                               StoreId = s.Id,
                                               StoreName = s.Name,
                                               StoreImage = s.Image,
                                               Slogan = s.Slogan,
                                               CategoryId = c.Id,
                                               ProductCategory = c.Name
                                           }
                                     ).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return listproductdetail;
        }

        public async Task<ProductDetailsModel> GetById(string proId)
        {
            var product = await (from p in context.Products
                                 join s in context.Stores on p.StoreId equals s.Id
                                 join c in context.Categories on p.CategoryId equals c.Id
                                 where p.Id == proId
                                 select new ProductDetailsModel()
                                 {
                                     Id = p.Id,
                                     Name = p.Name,
                                     Image = p.Image,
                                     Unit = p.Unit,
                                     PricePerPack = p.PricePerPack,
                                     PackNetWeight = p.PackNetWeight,
                                     PackDescription = p.PackDescription,
                                     MaximumQuantity = p.MaximumQuantity,
                                     MinimumQuantity = p.MinimumQuantity,
                                     Description = p.Description,
                                     MinimumDeIn = p.MinimumDeIn,
                                     Rate = p.Rate,
                                     StoreId = s.Id,
                                     StoreName = s.Name,
                                     StoreImage = s.Image,
                                     Slogan = s.Slogan,
                                     CategoryId = c.Id,
                                     ProductCategory = c.Name

                                 }).FirstOrDefaultAsync();

            return product;
        }
        public async Task<ProductModel> CreatNewProduct(ProductModel pro)
        {
            context.Products.Add(
                new Product {
                    Id = Guid.NewGuid().ToString(),
                    Name = pro.Name,
                    Image = await _fileService.UploadFile(pro.Image),
                    Unit = pro.Unit,
                    PricePerPack= pro.PricePerPack,
                    PackDescription= pro.PackDescription,
                    PackNetWeight= pro.PackNetWeight,
                    MaximumQuantity= pro.MaximumQuantity,
                    MinimumQuantity = pro.MinimumQuantity,
                    MinimumDeIn = pro.MinimumDeIn,
                    StoreId = pro.StoreId,
                    CategoryId = pro.CategoryId,
                    Rate = pro.Rate,
                    Description = pro.Description,
                    });
            await context.SaveChangesAsync();
            return pro;
        }
        public async Task<Object> UpdateProductDetailById(string proId, ProductDetailsModel product)
        {
            if (proId == null)
            {
                return null;
            }
            var pro = await context.Products.FindAsync(proId);
            var store = context.Stores.FirstOrDefault(s => s.Id == product.StoreId);
            var category = context.Categories.FirstOrDefault(c => c.Id == product.CategoryId);
            pro.Id = product.Id;
            pro.Name = product.Name;
            pro.Image = product.Image;
            pro.Unit = product.Unit;
            pro.PricePerPack = product.PricePerPack;
            pro.PackNetWeight = product.PackNetWeight;
            pro.PackDescription = product.PackDescription;
            pro.MaximumQuantity = product.MaximumQuantity;
            pro.MinimumQuantity = product.MinimumQuantity;
            pro.Description = product.Description;
            pro.Rate = product.Rate;
            pro.StoreId = product.StoreId;
            pro.CategoryId = product.CategoryId;

            var listProInMenu = await context.ProductInMenus.Where(pm => pm.ProductId == proId).ToListAsync();
            var listCateInMenu = await context.CategoryInMenus.Where(cm => cm.CategoryId == product.CategoryId).ToListAsync();
            if (listProInMenu.Any())
            {
                if (listCateInMenu.Any()) 

                context.CategoryInMenus.RemoveRange(listCateInMenu);
            }
            context.Entry(pro).State = EntityState.Modified; 
            try
                {
                    await context.SaveChangesAsync();
                }
                catch
                {
                    throw;
                } 
            return product;
            }
        public async Task<Object> DeleteProductById(string id)  
        {
            var product = await context.Products.FindAsync(id);
            if (product == null)
            {
                return null;
            }
            context.Products.Remove(product);
            await context.SaveChangesAsync();

            return product;
        }        
        public async Task<Object> PostFireBase(IFormFile file)
        {
            var fileUpload = file;
            FileStream fs = null;
            if (fileUpload.Length > 0)
            {
                {
                    string folderName = "ImagesProducts";
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
                    ).Child("ImageProduct").Child(fileUpload.FileName).PutAsync(fs, cancel.Token);
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
