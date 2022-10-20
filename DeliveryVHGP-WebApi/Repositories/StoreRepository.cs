using DeliveryVHGP_WebApi.IRepositories;
using DeliveryVHGP_WebApi.Models;
using DeliveryVHGP_WebApi.Services;
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
        private readonly ITimeStageService _timeStageService ;
        public StoreRepository(ITimeStageService timeStageService, IFileService fileService, DeliveryVHGP_DBContext context)
        {
            _context = context;
            _fileService = fileService;
            _timeStageService = timeStageService;
        }
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
                                       Status = store.Status,
                                       CreateAt = store.CreateAt,
                                       UpdateAt = store.UpdateAt

                                   }).OrderByDescending(t => t.CreateAt).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            foreach(var store in listStore)
            {
                store.Account = await GetAccountInStore(store.Id);
            }    
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
                                       Status = store.Status,
                                       CreateAt= store.CreateAt,
                                       UpdateAt= store.UpdateAt

                                   }).OrderByDescending(t => t.CreateAt).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
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
                                       Status = store.Status,
                                       CreateAt = store.CreateAt,
                                       UpdateAt = store.UpdateAt

                                   }).OrderByDescending(t => t.CreateAt).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            foreach (var store in listStore)
            {
                store.Account = await GetAccountInStore(store.Id);
            }
            return listStore;
        }
        public async Task<Object> GetStoreById(string storeId)
        {
            var store = await (from s in _context.Stores
                               join b in _context.Brands on s.BrandId equals b.Id
                               join sc in _context.StoreCategories on s.StoreCategoryId equals sc.Id
                               join bs in _context.Buildings on s.BuildingId equals bs.Id
                               where s.Id == storeId 
                               select new ViewListStoreModel()
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
                                   Status = s.Status,
                                   CreateAt = s.CreateAt,
                                   UpdateAt = s.UpdateAt,
                               }).FirstOrDefaultAsync();
            store.Account = await GetAccountInStore(storeId);
            return store;
        }
        public async Task<StoreDto> CreatNewStore(StoreDto store)
        {
            string fileImg = "ImagesStores";
            string time = await _timeStageService.GetTime();
            var categoryStore = _context.StoreCategories.FirstOrDefault(sc => sc.Id == store.StoreCategoryId);
            var brand = _context.Brands.FirstOrDefault(b => b.Id == store.BrandId);
            var building = _context.Buildings.FirstOrDefault(bs => bs.Id == store.BuildingId);
            _context.Stores.Add(
                new Store
                {
                    Id = store.Id,
                    Name = store.Name,
                    Phone = store.Phone,
                    Slogan = store.Slogan,
                    Image = await _fileService.UploadFile(fileImg, store.Image),
                    Rate = store.Rate,
                    OpenTime = store.OpenTime,
                    CloseTime = store.CloseTime,
                    StoreCategoryId = categoryStore.Id,
                    BrandId = brand.Id,
                    BuildingId = building.Id,
                    Status = true,
                    CreateAt = time
                });
            _context.Accounts.Add(
                new Account
                {
                    Id = store.Id,
                    Name = store.Name,
                    Password = store.Password,
                    RoleId = "2",
                    Status = "true"
                });
            await _context.SaveChangesAsync();
            return store;

        }
        public async Task<Object> DeleteStore(string storeId)
        {
            var deStore = await _context.Stores.FindAsync(storeId);
            _context.Stores.Remove(deStore);
            var account = await _context.Accounts.FindAsync(storeId);
            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();

            return deStore;
        }
        public async Task<AccountInRole> GetAccountInStore(string storeId)
        {
            var account = await _context.Accounts.Where(x => x.Id == storeId)
                                    .Select(x => new AccountInRole
                                    {
                                        Password = x.Password,
                                    }).FirstOrDefaultAsync();
            return account;
        }

        public async Task<StoreDto> UpdateStore(string storeId, StoreDto store , Boolean imgUpdate)
        {
            string fileImg = "ImagesStores";
            string time = await _timeStageService.GetTime();
            var result = await _context.Stores.FindAsync(storeId);
            var account = _context.Accounts.FirstOrDefault(x => x.Id == storeId);

            result.Id = store.Id;
            result.Name = store.Name;
            result.Rate = store.Rate;
            result.BrandId = store.BrandId;
            result.BuildingId = store.BuildingId;
            result.StoreCategoryId = store.StoreCategoryId;
            if (imgUpdate == true)
            {
                result.Image = await _fileService.UploadFile(fileImg, store.Image);
            }
            result.OpenTime = store.OpenTime;
            result.CloseTime = store.CloseTime;
            result.Phone = store.Phone;
            result.Slogan = store.Slogan;
            result.Status = store.Status;
            result.UpdateAt = time;
            account.Password = store.Password;

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
    }
}
