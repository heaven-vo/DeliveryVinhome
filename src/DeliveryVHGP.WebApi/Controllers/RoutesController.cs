using DeliveryVHGP.Core.Interfaces;
using DeliveryVHGP.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryVHGP.WebApi.Controllers
{
    [Route("api/v1/routes")]
    [ApiController]
    public class RoutesController : ControllerBase
    {
        private readonly IRepositoryWrapper repository;
        public RoutesController(IRepositoryWrapper repository)
        {
            this.repository = repository;
        }
        /// <summary>
        /// Get all routes(not assign and todo)
        /// </summary>
        [HttpGet("GetRoute")]
        public async Task<ActionResult<List<RouteModel>>> GetRoute()
        {
            var routes = await repository.RouteAction.GetCurrentAvalableRoute();
            return Ok(routes);
        }
        /// <summary>
        /// Shipper accept a route
        /// </summary>
        [HttpGet("{routeId}/accept")]
        public async Task<ActionResult> AcceptRoute(string routeId, string shipperId)
        {
            try
            {
                await repository.RouteAction.AcceptRouteByShipper(routeId, shipperId);
                return Ok(new { StatusCode = "Successful", data = 1 });
            }
            catch
            {
                return Ok(new
                {
                    StatusCode = "Fail",
                    message = "NotFound"
                });
            }

        }
        /// <summary>
        /// Get list edges in route
        /// </summary>
        [HttpGet("{routeId}/edges")]
        public async Task<ActionResult<List<EdgeModel>>> GetListEdgesInRoute(string routeId)
        {
            try
            {
                var listEdge = await repository.RouteAction.GetListEdgeInRoute(routeId);
                return Ok(new { StatusCode = "Successful", data = listEdge });
            }
            catch
            {
                return Ok(new
                {
                    StatusCode = "Fail",
                    message = "NotFound"
                });
            }
        }
    }
}
