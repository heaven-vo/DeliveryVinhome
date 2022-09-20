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

        public async Task<IEnumerable<StoreModel>> GetAll(int pageIndex, int pageSize)
        {
            double time = await GetTime();

            var listStore = await _context.Stores.
                Select(x => new StoreModel
                {

                    Id = x.Id,
                    Name = x.Name,
                    Image = x.Image,
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
