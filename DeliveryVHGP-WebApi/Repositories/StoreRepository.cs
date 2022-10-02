using DeliveryVHGP_WebApi.IRepositories;
using DeliveryVHGP_WebApi.Models;
using DeliveryVHGP_WebApi.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DeliveryVHGP_WebApi.Repositories
{
    public class StoreRepository : IStoreRepository
    {
        private readonly DeliveryVHGP_DBContext _context;

        public StoreRepository(DeliveryVHGP_DBContext context)
        {
            _context = context;
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

        public async Task<double> GetTime()
        {
            DateTime utcDateTime = DateTime.UtcNow;
            string vnTimeZoneKey = "SE Asia Standard Time";
            TimeZoneInfo vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById(vnTimeZoneKey);
            string time = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, vnTimeZone).ToString("HH.mm");
            var time2 = Double.Parse(time);
            return time2;
        }
    }
}
