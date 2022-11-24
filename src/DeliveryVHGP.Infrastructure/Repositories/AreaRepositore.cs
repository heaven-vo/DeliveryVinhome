using DeliveryVHGP.Core.Interface.IRepositories;
using DeliveryVHGP.Core.Data;
using DeliveryVHGP.Core.Models;
using Microsoft.EntityFrameworkCore;
using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Infrastructure.Repositories.Common;

namespace DeliveryVHGP.WebApi.Repositories
{
    public class AreaRepositore : RepositoryBase<Area>, IAreaRepository
    {
        public AreaRepositore(DeliveryVHGP_DBContext context): base(context)
        {
        }
        public async Task<List<AreaDto>> GetAll(int pageIndex, int pageSize)
        {
            var listAreas = await context.Areas.
                Select(x => new AreaDto
                {
                    Id = x.Id,
                    Name = x.Name,
                }).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return listAreas;
        }
        public async Task<Object> GetBuildingByArea(string areaId)
        {
            var listArea = await (from a in context.Areas
                                  where a.Id == areaId
                                      select new ViewListArea()
                                      {
                                          Id = a.Id,
                                          Name = a.Name,
                                      }).FirstOrDefaultAsync();
            var listcluster = await (from cl in context.Clusters
                                     join a in context.Areas on cl.AreaId equals a.Id
                                     where cl.AreaId == areaId
                                     select new ViewListClusterInArea
                                     {
                                         Id = cl.Id,
                                         Name = cl.Name
                                     }
                                     ).ToListAsync();
            foreach (var cluster in listcluster)
            {
                var listBuilding = await GetBuildingByCluster(cluster.Id);
                cluster.ListBuilding = listBuilding;
            }
            listArea.ListCluster = listcluster;

            return listArea;
        }
        public async Task<List<ViewListBuilding>> GetBuildingByCluster(string clusterId)
        {
            var listBuilding = await (from b in context.Buildings
                                      join cl in context.Clusters on b.ClusterId equals cl.Id
                                      where cl.Id == clusterId
                                      select new ViewListBuilding
                                      {
                                          Id = b.Id,
                                          Name = b.Name,
                                          HubId = b.HubId,
                                          Longitude = b.Longitude,
                                          Latitude = b.Latitude,
                                      }
                                     ).ToListAsync();
            return listBuilding;
        }
        public async Task<AreaModel> CreateArea(AreaModel area)
        {
            var id = Guid.NewGuid().ToString();
            var newArea = new Area
            { 
                Id = id,
                Name = area.Name,
            };
            context.Areas.Add(newArea);

            List<Cluster> list = new List<Cluster>();
            foreach (var cluster in area.listCluster)
            {
                var newCluster = new Cluster
                {
                   Id = Guid.NewGuid().ToString(),
                  Name = cluster.Name,
                    AreaId = id,
                };
                list.Add(newCluster);
            }
            context.Clusters.AddRangeAsync(list);
            await context.SaveChangesAsync();
            return area;
        }
        public async Task<Object> UpdateAreaById(string areaId, AreaDto area)
        {
            if (areaId == null)
            {
                return null;
            }
            var result = await context.Areas.FindAsync(areaId);
            result.Id = area.Id;
            result.Name = area.Name;

            var clusterr = context.Clusters.Where(c => c.AreaId == areaId).ToList();
            if (clusterr.Any())
            {
                context.Clusters.RemoveRange(clusterr);
            }
            List<Cluster> list = new List<Cluster>();
            foreach (var cluster in area.listCluster)
            {
                var newCluster = new Cluster
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = cluster.Name,
                    AreaId = areaId,
                };
                list.Add(newCluster);
            }
            context.Clusters.AddRangeAsync(list);
            context.Entry(result).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
            return area;
        }
        public async Task<Object> DeleteById(string areaId)
        {
            var area = await context.Areas.FindAsync(areaId);
            context.Areas.Remove(area);
            await context.SaveChangesAsync();

            return area;

        }
    }
}
