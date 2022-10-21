using DeliveryVHGP.Core.Interface.IRepositories;
using DeliveryVHGP.Core.Data;
using DeliveryVHGP.Core.Models;
using Microsoft.EntityFrameworkCore;
using DeliveryVHGP.Infrastructure.Services;
using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Infrastructure.Repositories.Common;

namespace DeliveryVHGP.WebApi.Repositories
{
    public class StoreRepository : RepositoryBase<Store>, IStoreRepository
    {
        private readonly IFileService _fileService;
        private readonly ITimeStageService _timeStageService ;
        public StoreRepository(ITimeStageService timeStageService, IFileService fileService, DeliveryVHGP_DBContext context): base(context)
        {
            _fileService = fileService;
            _timeStageService = timeStageService;
        }
        public async Task<IEnumerable<StoreModel>> GetListStore(int pageIndex, int pageSize)
        {
            var listStore = await (from store in context.Stores
                                   join b in context.Brands on store.BrandId equals b.Id
                                   join building in context.Buildings on store.BuildingId equals building.Id
                                   join sc in context.StoreCategories on store.StoreCategoryId equals sc.Id
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
            var listStore = await (from store in context.Stores
                                   join b in context.Brands on store.BrandId equals b.Id
                                   join building in context.Buildings on store.BuildingId equals building.Id
                                   join sc in context.StoreCategories on store.StoreCategoryId equals sc.Id
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
            var listStore = await (from store in context.Stores.Where(store => store.Name.Contains(storeName))
                                   join b in context.Brands on store.BrandId equals b.Id
                                   join building in context.Buildings on store.BuildingId equals building.Id
                                   join sc in context.StoreCategories on store.StoreCategoryId equals sc.Id
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
            var store = await (from s in context.Stores
                               join b in context.Brands on s.BrandId equals b.Id
                               join sc in context.StoreCategories on s.StoreCategoryId equals sc.Id
                               join bs in context.Buildings on s.BuildingId equals bs.Id
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
            var categoryStore = context.StoreCategories.FirstOrDefault(sc => sc.Id == store.StoreCategoryId);
            var brand = context.Brands.FirstOrDefault(b => b.Id == store.BrandId);
            var building = context.Buildings.FirstOrDefault(bs => bs.Id == store.BuildingId);
            context.Stores.Add(
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
            context.Accounts.Add(
                new Account
                {
                    Id = store.Id,
                    Name = store.Name,
                    Password = store.Password,
                    RoleId = "2",
                    Status = "true"
                });
            await context.SaveChangesAsync();
            return store;

        }
        public async Task<Object> DeleteStore(string storeId)
        {
            var deStore = await context.Stores.FindAsync(storeId);
            context.Stores.Remove(deStore);
            var account = await context.Accounts.FindAsync(storeId);
            context.Accounts.Remove(account);
            await context.SaveChangesAsync();

            return deStore;
        }
        public async Task<AccountInRole> GetAccountInStore(string storeId)
        {
            var account = await context.Accounts.Where(x => x.Id == storeId)
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
            var result = await context.Stores.FindAsync(storeId);
            var account = context.Accounts.FirstOrDefault(x => x.Id == storeId);

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
                await context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
            return store;
        }
        public async Task<StatusStoreDto> UpdateStatusStore(string storeId, StatusStoreDto store)
        {
            var result = await _context.Stores.FindAsync(storeId);
            var status =  _context.Orders.FirstOrDefault(x => x.StoreId == storeId);
            var OrderStatus = _context.OrderStatuses.FirstOrDefault(os => os.Id == status.StatusId);

            result.Id = store.Id;
            if (status.StatusId == "4" || status.StatusId == "5")
            {
                result.Status = store.Status;
            }
            if (status.StatusId == "1" || status.StatusId == "2" || status.StatusId == "3")
                throw new Exception("Hiện tại cửa hàng đang có đơn hàng trong menu!!" +
                                             "Vui lòng kiểm tra lại đơn hàng và thử lại");
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
