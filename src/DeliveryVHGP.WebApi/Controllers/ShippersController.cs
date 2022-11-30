using DeliveryVHGP.Core.Interfaces;
using DeliveryVHGP.Core.Models;
using Microsoft.AspNetCore.Mvc;
using static DeliveryVHGP.Core.Models.OrderAdminDto;

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
        [HttpGet("{shipperId}/wallet")]
        public async Task<ActionResult> GetBalaceWallet(string shipperId)
        {
            try
            {
                var balance = await repository.Transaction.GetBalanceWallet(shipperId);
                return Ok(new { StatusCode = "Successful", data = balance });
            }
            catch (Exception e)
            {
                return Ok(new
                {
                    StatusCode = "Fail",
                    message = e.Message
                });
            }
        }
        [HttpGet("{shipperId}/report")]
        public async Task<ActionResult<ShipperReportModel>> GetOrderReport(string shipperId, [FromQuery] DateFilterRequest request, [FromQuery] MonthFilterRequest monthFilter)
        {
            try
            {
                var report = await repository.ShipperHistory.GetShipperReport(shipperId, request, monthFilter);
                return Ok(new { StatusCode = "Successful", data = report });
            }
            catch (Exception e)
            {
                return Ok(new
                {
                    StatusCode = "Fail",
                    message = e.Message
                });
            }
        }
    }
}
