using DeliveryVHGP.Core.Interfaces;
using DeliveryVHGP.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryVHGP.WebApi.Controllers
{
    [Route("api/v1/areas")]
    [ApiController]
    public class AreaController : ControllerBase
    {
        private readonly IRepositoryWrapper repository;
        public AreaController(IRepositoryWrapper repository)
        {
            this.repository = repository;
        }
        [HttpPost("menu3")]
        public async Task<ActionResult> CreateMenuMode3()
        {
            await repository.Menu.CreateMenuMode3();
            return Ok();
        }
        //[HttpGet("Route")]
        //public async Task<ActionResult> CreateRoute()
        //{
        //    List<SegmentDeliveryRoute> list = new List<SegmentDeliveryRoute>();
        //    SegmentDeliveryRoute routeA = new SegmentDeliveryRoute() { Id = Guid.NewGuid().ToString(), Distance = 10, Status = 1 };
        //    routeA.RouteEdges = new List<RouteEdge>() {
        //        new RouteEdge() { Id = Guid.NewGuid().ToString(), RouteId = routeA.Id, Priority = 1,},
        //        new RouteEdge() { Id = Guid.NewGuid().ToString(), RouteId = routeA.Id, Priority = 2}
        //    };
        //    SegmentDeliveryRoute routeB = new SegmentDeliveryRoute() { Id = Guid.NewGuid().ToString(), Distance = 10, Status = 1 };

        //    List<RouteEdge> edgeB = new List<RouteEdge>() {
        //        new RouteEdge() { Id = Guid.NewGuid().ToString(), RouteId = routeB.Id, Priority = 3},
        //        new RouteEdge() { Id = Guid.NewGuid().ToString(), RouteId = routeB.Id, Priority = 4}
        //    };
        //    routeB.RouteEdges = edgeB;

        //    list.Add(routeA);
        //    list.Add(routeB);
        //    await repository.RouteAction.CreateRoute(list);
        //    return Ok();
        //}
        [HttpGet("remove")]
        public async Task<ActionResult> RemoveAllRoute()
        {
            await repository.RouteAction.RemoveAllRouteAction();
            return Ok();
            //return Ok(await repository.Order.CheckAvailableOrder());
        }
        [HttpGet("check")]
        public async Task<ActionResult> CheckOrder()
        {
            var listOrder = await repository.Cache.GetOrderFromCache(15, 2);
            return Ok(await repository.Segment.GetSegmentAvaliable(listOrder));
            //return Ok(await repository.Order.CheckAvailableOrder());
        }
        /// <summary>
        /// Get list all Bulding with pagination
        /// </summary>
        //GET: api/v1/Brand?pageIndex=1&pageSize=3
        [HttpGet]
        public async Task<ActionResult> GetAll(int pageIndex, int pageSize)
        {
            return Ok(await repository.Area.GetAll(pageIndex, pageSize));
        }
        /// <summary>
        /// Get building by Areaid with pagination
        /// </summary>
        //GET: api/v1/buildingbyAreaId?pageIndex=1&pageSize=3
        [HttpGet("ByAreaId")]
        public async Task<ActionResult> GetBuildingByAreaId(string areaId)
        {
            var building = await repository.Area.GetBuildingByArea(areaId);
            if (building == null)
                return NotFound();
            return Ok(building);
        }
        /// <summary>
        /// Create a Area 
        /// </summary>
        //POST: api/v1/area
        [HttpPost]
        public async Task<ActionResult> CreateArea(AreaModel area)
        {
            try
            {
                var result = await repository.Area.CreateArea(area);
                return Ok(result);
            }
            catch
            {
                return Conflict();
            }
        }
        /// <summary>
        /// Delete a area by id
        /// </summary>
        //DELETE: api/v1/area/{id}
        [HttpDelete("{areaId}")]
        public async Task<ActionResult> DeleteArea(string areaId)
        {
            try
            {
                var result = await repository.Area.DeleteById(areaId);
                return Ok(result);
            }
            catch
            {
                return Conflict();
            }

        }
        /// <summary>
        /// Update Area with pagination
        /// </summary>
        //PUT: api/v1/Area?id
        [HttpPut("{areaId}")]
        public async Task<ActionResult> UpdateArea(string areaId, AreaDto area)
        {
            try
            {
                if (areaId != area.Id)
                {
                    return BadRequest("Area ID mismatch");
                }
                var BrandToUpdate = await repository.Area.UpdateAreaById(areaId, area);
                return Ok(area);
            }
            catch
            {
                return Conflict();
            }
        }
    }
}
