using DeliveryVHGP.Core.Data;
using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Core.Enums;
using DeliveryVHGP.Core.Interfaces.IRepositories;
using DeliveryVHGP.Core.Models;
using DeliveryVHGP.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace DeliveryVHGP.Infrastructure.Repositories
{
    public class SegmentRepository : RepositoryBase<Segment>, ISegmentRepository
    {
        public SegmentRepository(DeliveryVHGP_DBContext context) : base(context)
        {
        }
        public async Task CreatSegment(OrderDto order)
        {
            var storeBuildingId = await context.Stores.Where(x => x.Id == order.StoreId).Select(x => x.BuildingId).FirstOrDefaultAsync();
            var hubId = await context.Buildings.Where(x => x.Id == storeBuildingId).Select(x => x.HubId).FirstOrDefaultAsync();
            if (order.ServiceId == "0")
            {
                Segment toHubSegment = new Segment()
                {
                    Id = Guid.NewGuid().ToString(),
                    FromBuildingId = storeBuildingId,
                    ToBuildingId = hubId,
                    HubId = hubId,
                    OrderId = order.Id,
                    SegmentMode = (int)SegmentModeEnum.StoreToHub,
                    Status = (int)SegmentStatusEnum.Viable
                };
                Segment toCusSegment = new Segment()
                {
                    Id = Guid.NewGuid().ToString(),
                    FromBuildingId = hubId,
                    ToBuildingId = order.BuildingId,
                    HubId = hubId,
                    OrderId = order.Id,
                    SegmentMode = (int)SegmentModeEnum.HubToCus,
                    Status = (int)SegmentStatusEnum.Unviable
                };
                await context.Segments.AddRangeAsync(toHubSegment, toCusSegment);
            }
            if (order.ServiceId == "1")
            {
                Segment storeToCusSegment = new Segment()
                {
                    Id = Guid.NewGuid().ToString(),
                    FromBuildingId = storeBuildingId,
                    ToBuildingId = order.BuildingId,
                    HubId = hubId,
                    OrderId = order.Id,
                    SegmentMode = (int)SegmentModeEnum.StoreToCus,
                    Status = (int)SegmentStatusEnum.Viable
                };
                await Add(storeToCusSegment);
            }
            await Save();
        }
        public async Task<List<SegmentModel>> GetSegmentAvaliable(List<string> listOrder)
        {
            List<SegmentModel> listVetorBuilding = new List<SegmentModel>();
            var listSegment = await context.Segments.Where(x => listOrder.Contains(x.OrderId) && x.Status == (int)SegmentStatusEnum.Viable)
               .ToListAsync();
            if (listSegment != null)
            {
                //listSegment.ForEach(x => x.Status = (int)SegmentStatusEnum.Unviable); //ko unviable nua, reroute khoi phai bat viable, segment done thoi
                listVetorBuilding = listSegment.Select(x => new SegmentModel
                {
                    SegmentId = x.Id,
                    OrderId = x.OrderId,
                    fromBuilding = x.FromBuildingId,
                    toBuilding = x.ToBuildingId,
                    SegmentMode = x.SegmentMode
                }).ToList();
                await Save();
            }

            return listVetorBuilding;
        }
    }
}
