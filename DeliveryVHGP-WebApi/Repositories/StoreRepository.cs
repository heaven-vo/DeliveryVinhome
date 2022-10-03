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
        public async Task<Object> GetStoreById(string storeId)
        {
            var store = await (from s in _context.Stores
                               join b in _context.Brands on s.BrandId equals b.Id
                               join sc in _context.StoreCategories on s.StoreCategoryId equals sc.Id
                               join bs in _context.Buildings on s.BuildingId equals bs.Id
                               where s.Id == storeId
                               select new StoreModel()
                               {
                                   Id = s.Id,
                                   Name = s.Name,
                                   Phone = s.Phone,
                                   BrandStoreId = b.Id,
                                   BrandStoreName = b.Name,
                                   BuildingId = bs.Id,
                                   BuildingStore = bs.Name,
                                   StoreCateId = sc.Id,
                                   StoreCateName = sc.Name,
                                   Status = s.Status
                               }).FirstOrDefaultAsync();
            return store;
        }
        //public async Task<ProductModel> CreatNewProduct(ProductModel pro)
        //{
        //    var store = _context.Stores.FirstOrDefault(x => x.Id == pro.StoreId);
        //    var cate = _context.Categories.FirstOrDefault(c => c.Id == pro.CategoryId);
        //    _context.Products.Add(
        //        new Product()
        //        {
        //            Id = pro.Id,
        //            Name = pro.Name,
        //            Image = pro.Image,
        //            Unit = pro.Unit,
        //            PricePerPack = pro.PricePerPack,
        //            PackDescription = pro.PackDescription,
        //            PackNetWeight = pro.PackNetWeight,
        //            MaximumQuantity = pro.MaximumQuantity,
        //            MinimumQuantity = pro.MinimumQuantity,
        //            MinimumDeIn = pro.MinimumDeIn,
        //            Rate = pro.Rate,
        //            Description = pro.Description,
        //            StoreId = store.Id,
        //            CategoryId = cate.Id,
        //        });
        //    await _context.SaveChangesAsync();
        //    return pro;
        //}
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
                    Image = store.Image,
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

        public async Task<StoreDto> UpdateStore(string storeId , StoreDto store)
        {
            var result = await _context.Stores.FindAsync(storeId);
            var brand = _context.Brands.Where(b => b.Id != result.BrandId).Select(b => b.Id).FirstOrDefault();
            var building = _context.Buildings.Where(bs => bs.Id != result.BuildingId).Select(bs => bs.Id ).FirstOrDefault(); 
            var storeCate = _context.StoreCategories.Where(sc => sc.Id != result.StoreCategoryId).Select(sc => sc.Id).FirstOrDefault(); 


            result.Name = store.Name;
            result.Rate = store.Rate;
            result.Image = store.Image;
            result.OpenTime = store.OpenTime;
            result.CloseTime = store.CloseTime;
            result.Phone = store.Phone;
            result.Slogan = store.Slogan;
            result.BrandId = brand.ToString();
            result.BuildingId = building.ToString();
            result.StoreCategoryId = storeCate.ToString();
            result.Status = store.Status;

            _context.Entry(result).State = EntityState.Modified;


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
