using DeliveryVHGP.Core.Interface.IRepositories;
using DeliveryVHGP.Core.Data;
using DeliveryVHGP.Core.Models;
using Microsoft.EntityFrameworkCore;
using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Infrastructure.Repositories.Common;

namespace DeliveryVHGP.WebApi.Repositories
{
    public class BuildingRepository : RepositoryBase<Building>, IBuildingRepository
    {
        public BuildingRepository(DeliveryVHGP_DBContext context): base(context)
        {
        }
        public async Task<List<ViewListBuilding>> GetAll(int pageIndex, int pageSize)
        {
            var listBuilding = await context.Buildings.
                Select(x => new ViewListBuilding
                {
                    Id = x.Id,
                    Name = x.Name,
                    Longitude = x.Longitude,
                    Latitude = x.Latitude,
                }).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            
            return listBuilding;
        }
        public async Task<Object> GetBuildinById(string buildingId)
        {
            var building = await context.Buildings.Where(x => x.Id == buildingId)
                                     .Select(x => new ViewListBuilding
                                     {
                                         Id = x.Id,
                                         Name = x.Name,
                                         Longitude = x.Longitude,
                                         Latitude = x.Latitude,
                                     }).FirstOrDefaultAsync();
            return building;
        }
        public async Task<BuildingModel> CreateBuilding(BuildingModel building)
        {
            context.Buildings.Add(
                new Building
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = building.Name,
                }
                );
            await context.SaveChangesAsync();
            return building;

        }
        public async Task<BuildingDto> UpdateLongLatBuilding(string buildingId, BuildingDto building)
        {
            var result = await context.Buildings.FindAsync(buildingId);

            result.Longitude = building.Longitude;
            result.Latitude = building.Latitude;

        
            try
            {
                context.Entry(result).State = EntityState.Modified;
                await context.SaveChangesAsync();
                }
             catch
            {
                  throw;
            }
                return building;
        }
    }
}
