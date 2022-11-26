using DeliveryVHGP.Core.Interface.IRepositories;
using DeliveryVHGP.Core.Data;
using DeliveryVHGP.Core.Models;
using Microsoft.EntityFrameworkCore;
using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Infrastructure.Repositories.Common;

namespace DeliveryVHGP.WebApi.Repositories
{
    public class HubRepository : RepositoryBase<Hub>, IHubRepository

    {
        public HubRepository(DeliveryVHGP_DBContext context) : base(context)
        {

        }
        public async Task<IEnumerable<HubModels>> GetlistHub(int pageIndex, int pageSize , FilterRequestInHub request)
        {
            var newHub = await context.Hubs.Where(hub => hub.Name.Contains(request.SearchByName))
                .Select(x => new HubModels
                {
                    Id = x.Id,
                    Name = x.Name,
                    BuildingId = x.BuildingId,
                }).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return newHub;
        }
        public async Task<HubModels> GetById(string id)
        {
            var hub = await context.Hubs.Where(x => x.Id == id).Select(x => new HubModels
            {
                Id = x.Id,
                Name = x.Name,
                BuildingId = x.BuildingId
            }).FirstOrDefaultAsync();
            return hub;
        }
        public async Task<HubDto> CreateHub(HubDto hub)
        {
            context.Hubs.Add(new Hub { Id = Guid.NewGuid().ToString(), Name = hub.Name, BuildingId = hub.BuildingId});

            await context.SaveChangesAsync();
            return hub;
        }
        public async Task<Object> DeleteById(string hubId)
        {
            var hub = await context.Hubs.FindAsync(hubId);
            context.Hubs.Remove(hub);
            await context.SaveChangesAsync();

            return hub;
        }

        public async Task<Object> UpdateHubById(string hubId, HubModels hub)
        {
            if (hubId == null)
            {
                return null;
            }
            var result = await context.Hubs.FindAsync(hubId);
            result.Id = hub.Id;
            result.Name = hub.Name;
            result.BuildingId = hub.BuildingId;

            context.Entry(result).State = EntityState.Modified;
            try
            {
                await context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
            return hub;
        }
    }
}
