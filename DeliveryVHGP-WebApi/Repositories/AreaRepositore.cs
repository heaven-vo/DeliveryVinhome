using DeliveryVHGP_WebApi.IRepositories;
using DeliveryVHGP_WebApi.Models;
using DeliveryVHGP_WebApi.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DeliveryVHGP_WebApi.Repositories
{
    public class AreaRepositore : IAreaRepository
    {
        private readonly DeliveryVHGP_DBContext _context;
        public AreaRepositore(DeliveryVHGP_DBContext context)
        {
            _context = context;
        }
        public async Task<List<AreaDto>> GetAll(int pageIndex, int pageSize)
        {
            var listAreas = await _context.Areas.
                Select(x => new AreaDto
                {
                    Id = x.Id,
                    Name = x.Name,
                }).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return listAreas;
        }
        public async Task<Object> GetBuildingByArea(string areaId)
        {
            var listArea = await (from a in _context.Areas
                                  where a.Id == areaId
                                      select new ViewListArea()
                                      {
                                          Id = a.Id,
                                          Name = a.Name,
                                      }).FirstOrDefaultAsync();
            var listcluster = await (from cl in _context.Clusters
                                     join a in _context.Areas on cl.AreaId equals a.Id
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
            var listBuilding = await (from b in _context.Buildings
                                      join cl in _context.Clusters on b.ClusterId equals cl.Id
                                      where cl.Id == clusterId
                                      select new ViewListBuilding
                                      {
                                          Id = b.Id,
                                          Name = b.Name,
                                      }
                                     ).ToListAsync();
            return listBuilding;
        }
        public async Task<AreaModel> CreateArea(AreaModel area)
        {
            _context.Areas.Add(
                new Area
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = area.Name,   
                }
                );
            await _context.SaveChangesAsync();
            return area;
        }
        public async Task<Object> UpdateAreaById(string areaId, AreaDto area)
        {
            if (areaId == null)
            {
                return null;
            }
            var result = await _context.Areas.FindAsync(areaId);
            result.Id = area.Id;
            result.Name = area.Name;

            _context.Entry(result).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
            return area;
        }
        public async Task<Object> DeleteById(string areaId)
        {
            var area = await _context.Areas.FindAsync(areaId);
            _context.Areas.Remove(area);
            await _context.SaveChangesAsync();

            return area;

        }
    }
}
