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
                }).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            
            return listBuilding;
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
    }
}
