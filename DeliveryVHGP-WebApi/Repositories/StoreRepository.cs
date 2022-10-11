using DeliveryVHGP_WebApi.IRepositories;
using DeliveryVHGP_WebApi.Models;
using DeliveryVHGP_WebApi.ViewModels;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.EntityFrameworkCore;

namespace DeliveryVHGP_WebApi.Repositories
{
    public class StoreRepository : IStoreRepository
    {
        private readonly DeliveryVHGP_DBContext _context;
        private readonly IFileService _fileService;
        public StoreRepository(IFileService fileService, DeliveryVHGP_DBContext context)
        {
            _context = context;
            _fileService = fileService;
        }
        private static string apiKey = "AIzaSyAauR7Lp1qtRLPIOkONgrLyPYLrdjN_qKw";
        private static string apibucket = "lucky-science-341916.appspot.com";
        private static string authenEmail = "adminstore2@gmail.com";
        private static string authenPassword = "store123456";
        public async Task<IEnumerable<StoreModel>> GetListStore(int pageIndex, int pageSize)
        {
            var listStore = await (from store in _context.Stores
                                   join b in _context.Brands on store.BrandId equals b.Id
                                   join building in _context.Buildings on store.BuildingId equals building.Id
                                   join sc in _context.StoreCategories on store.StoreCategoryId equals sc.Id
                                   select new StoreModel()
                                   {
                                       Id = store.Id,
                                       Name = store.Name,
                                       Phone = store.Phone,
                                       BrandStoreId = b.Id,
                                       BrandStoreName = b.Name,
                                       BuildingId = building.Id,
                                       BuildingStore = building.Name,
                                       StoreCateId = sc.Id,
                                       StoreCateName = sc.Name,
                                       Status = store.Status

                                   }).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return listStore;
        }
        public async Task<IEnumerable<StoreModel>> GetListStoreInBrand(string brandName, int pageIndex, int pageSize)
        {
            var listStore = await (from store in _context.Stores
                                   join b in _context.Brands on store.BrandId equals b.Id
                                   join building in _context.Buildings on store.BuildingId equals building.Id
                                   join sc in _context.StoreCategories on store.StoreCategoryId equals sc.Id
                                   where b.Name.Contains(brandName)
                                   select new StoreModel()
                                   {
                                       Id = store.Id,
                                       Name = store.Name,
                                       Phone = store.Phone,
                                       BrandStoreId = b.Id,
                                       BrandStoreName = b.Name,
                                       BuildingId = building.Id,
                                       BuildingStore = building.Name,
                                       StoreCateId = sc.Id,
                                       StoreCateName = sc.Name,
                                       Status = store.Status

                                   }).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return listStore;
        }
        public async Task<IEnumerable<StoreModel>> GetListStoreByName(string storeName, int pageIndex, int pageSize)
        {
            var listStore = await (from store in _context.Stores.Where(store => store.Name.Contains(storeName))
                                   join b in _context.Brands on store.BrandId equals b.Id
                                   join building in _context.Buildings on store.BuildingId equals building.Id
                                   join sc in _context.StoreCategories on store.StoreCategoryId equals sc.Id
                                   select new StoreModel()
                                   {
                                       Id = store.Id,
                                       Name = store.Name,
                                       Phone = store.Phone,
                                       BrandStoreId = b.Id,
                                       BrandStoreName = b.Name,
                                       BuildingId = building.Id,
                                       BuildingStore = building.Name,
                                       StoreCateId = sc.Id,
                                       StoreCateName = sc.Name,
                                       Status = store.Status

                                   }).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return listStore;
        }
        public async Task<Object> GetStoreById(string storeId)
        {
            var store = await (from s in _context.Stores
                               join b in _context.Brands on s.BrandId equals b.Id
                               join sc in _context.StoreCategories on s.StoreCategoryId equals sc.Id
                               join bs in _context.Buildings on s.BuildingId equals bs.Id
                               where s.Id == storeId
                               select new StoreDto()
                               {
                                   Id = s.Id,
                                   Name = s.Name,
                                   Phone = s.Phone,
                                   Image = s.Image,
                                   CloseTime = s.CloseTime,
                                   OpenTime = s.OpenTime,
                                   Slogan = s.Slogan,
                                   BrandId = b.Id,
                                   BuildingId = bs.Id,
                                   StoreCategoryId = sc.Id,
                                   Status = s.Status
                               }).FirstOrDefaultAsync();
            return store;
        }
        public async Task<StoreDto> CreatNewStore(StoreDto store)
        {
            var categoryStore = _context.StoreCategories.FirstOrDefault(sc => sc.Id == store.StoreCategoryId);
            var brand = _context.Brands.FirstOrDefault(b => b.Id == store.BrandId);
            var building = _context.Buildings.FirstOrDefault(bs => bs.Id == store.BuildingId);
            _context.Stores.Add(
                new Store
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = store.Name,
                    Phone = store.Phone,
                    Slogan = store.Slogan,
                    Image = await _fileService.UploadFile(store.Image),
                    Rate = store.Rate,
                    OpenTime = store.OpenTime,
                    CloseTime = store.CloseTime,
                    StoreCategoryId = categoryStore.Id,
                    BrandId = brand.Id,
                    BuildingId = building.Id
                });

            await _context.SaveChangesAsync();
            return store;

        }
        public async Task<Object> DeleteStore(string storeId)
        {
            var deStore = await _context.Stores.FindAsync(storeId);
            _context.Stores.Remove(deStore);
            await _context.SaveChangesAsync();

            return deStore;
        }

        public async Task<StoreDto> UpdateStore(string storeId, StoreDto store)
        {

            var result = await _context.Stores.FindAsync(storeId);
            var brand = _context.Brands.FirstOrDefault(b => b.Id == store.BrandId);
            var building = _context.Buildings.FirstOrDefault(bs => bs.Id == store.BuildingId);
            var storeCate = _context.StoreCategories.FirstOrDefault(sc => sc.Id == store.StoreCategoryId);

            result.Name = store.Name;
            result.Rate = store.Rate;
            result.BrandId = store.BrandId;
            result.BuildingId = store.BuildingId;
            result.StoreCategoryId = store.StoreCategoryId;
            result.Image = store.Image;
            result.OpenTime = store.OpenTime;
            result.CloseTime = store.CloseTime;
            result.Phone = store.Phone;
            result.Slogan = store.Slogan;
            result.Status = store.Status;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
            return store;
        }
        public async Task<string> PostFireBase(IFormFile file)
        {
                var fileUpload = file;
                FileStream fs = null;
                if (fileUpload.Length > 0)
                {
                    {
                        string folderName = "ImagesStores";
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
                        ).Child("ImageStore").Child(fileUpload.FileName).PutAsync(fs, cancel.Token);
                    try
                    {
                        string Link = await upload;
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            return null;
        }
    }
}
