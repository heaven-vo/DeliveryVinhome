using Microsoft.AspNetCore.Mvc;
using DeliveryVHGP.Core.Interfaces;
using DeliveryVHGP.Core.Models;

namespace DeliveryVHGP.WebApi.Controllers
{
    [Route("api/v1/shipper-management/shippers")]
    [ApiController]
    public class ShippersManagementController : ControllerBase
    {
        private readonly IRepositoryWrapper repository;
        public ShippersManagementController(IRepositoryWrapper repository)
        {
            this.repository = repository;
        }
        /// <summary>
        /// Get list all shipper with pagination
        /// </summary>
        //GET: api/v1/shipper?pageIndex=1&pageSize=3
        [HttpGet]
        public async Task<ActionResult> GetAll(int pageIndex, int pageSize)
        {
            return Ok(await repository.Shipper.GetListShipper(pageIndex, pageSize));
        }
        /// <summary>
        /// Get Shipper by id with pagination
        /// </summary>
        //GET: api/v1/Shipper?pageIndex=1&pageSize=3
        [HttpGet("ByShipId")]
        public async Task<ActionResult<ShipperModel>> GetCategoryById(string id)
        {
            var ship = await repository.Shipper.GetShipperById(id);
            if (ship == null)
                return NotFound();
            return Ok(ship);
        }
        /// <summary>
        /// Create a shipper
        /// </summary>
        //POST: api/v1/shipper
        [HttpPost]
        public async Task<ActionResult> CreateShipper(ShipperDto shipper)
        {
            try
            {
                var result = await repository.Shipper.CreateShipper(shipper);
                return Ok(result);
            }
            catch
            {
                return Conflict();
            }
        }
    }
}
