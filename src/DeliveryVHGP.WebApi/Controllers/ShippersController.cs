using DeliveryVHGP.Core.Interfaces;
using DeliveryVHGP.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryVHGP.WebApi.Controllers
{
    [Route("api/v1/shippers")]
    [ApiController]
    public class ShippersController : ControllerBase
    {
        private readonly IRepositoryWrapper repository;
        public ShippersController(IRepositoryWrapper repository)
        {
            this.repository = repository;
        }
        [HttpGet("{shipperId}/current-job")]
        public async Task<ActionResult<EdgeModel>> GetCurrentEdgeOfShipper(string shipperId)
        {
            try
            {
                var edge = await repository.RouteAction.GetCurrentEdgeInRoute(shipperId);
                if (edge == null)
                {
                    return NoContent();
                }
                return Ok(edge);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
