using DeliveryVHGP_WebApi.IRepositories;
using DeliveryVHGP_WebApi.Models;
using DeliveryVHGP_WebApi.ViewModels;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.EntityFrameworkCore;

namespace DeliveryVHGP_WebApi.Repositories
{
    public class BuildingRepository : IBuildingRepository
    {
        private readonly DeliveryVHGP_DBContext _context;
        public BuildingRepository(DeliveryVHGP_DBContext context)
        {
            _context = context;
        }
        public async Task<List<ViewListBuilding>> GetAll(int pageIndex, int pageSize)
        {
            var listBuilding = await _context.Buildings.
                Select(x => new ViewListBuilding
                {
                    Id = x.Id,
                    Name = x.Name,
                }).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            
            return listBuilding;
        }
        public async Task<BuildingModel> CreateBuilding(BuildingModel building)
        {
            _context.Buildings.Add(
                new Building
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = building.Name,
                }
                );
            await _context.SaveChangesAsync();
            return building;

        }


    }
}
