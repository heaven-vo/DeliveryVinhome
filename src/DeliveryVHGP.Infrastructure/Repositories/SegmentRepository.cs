using DeliveryVHGP.Core.Data;
using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Core.Enums;
using DeliveryVHGP.Core.Interface.IRepositories;
using DeliveryVHGP.Core.Interfaces.IRepositories;
using DeliveryVHGP.Core.Models;
using DeliveryVHGP.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if(order.ServiceId == "0")
            {
                Segment toHubSegment = new Segment()
                {
                    Id = Guid.NewGuid().ToString(),
                    FromBuildingId = storeBuildingId,
                    ToBuildingId = hubId,
                    HubId = hubId,
                    OrderId = order.Id,
                    SegmentMode = (int)SegmentModeEnum.StoreToHub,
                    Status = (int)SegmentStatusEnum.NotAssign
                };
                Segment toCusSegment = new Segment()
                {
                    Id = Guid.NewGuid().ToString(),
                    FromBuildingId = hubId,
                    ToBuildingId = order.BuildingId,
                    HubId = hubId,
                    OrderId = order.Id,
                    SegmentMode = (int)SegmentModeEnum.HubToCus,
                    Status = (int)SegmentStatusEnum.NotAssign
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
                    Status = (int)SegmentStatusEnum.NotAssign
                };
                await Add(storeToCusSegment);
            }
            await Save();
        }
    }
}
