using DeliveryVHGP.Core.Interfaces;
using DeliveryVHGP.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryVHGP.WebApi.Controllers
{
    [Route("api/v1/routes")]
    [ApiController]
    public class EdgesController : ControllerBase
    {
        private readonly IRepositoryWrapper repository;
        public EdgesController(IRepositoryWrapper repository)
        {
            this.repository = repository;
        }
        [HttpGet("{edgeId}")]
        public async Task<ActionResult<List<OrderActionModel>>> GetActionInEdge(string edgeId)
        {
            //779527ca-568a-4e58-a615-e3a4ebc7b924  7b1f4cf1-0243-4478-b416-3a616e7ca4d2
            try
            {
                var listAction = await repository.RouteAction.GetListOrderAction(edgeId);
                return Ok(new { StatusCode = "Successful", data = listAction });
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
