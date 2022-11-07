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
        /// Get list all shipper by name with pagination
        /// </summary>
        //GET: api/v1/ShipperByName?pageIndex=1&pageSize=3
        [HttpGet("search-name")]
        public async Task<ActionResult> GetListShipperByName(string shipName, int pageIndex, int pageSize)
        {
            return Ok(await repository.Shipper.GetListShipperByName(shipName, pageIndex, pageSize));
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
        /// <summary>
        /// Update shipper  with pagination
        /// </summary>
        //PUT: api/v1/shipper?id
        [HttpPut("{shipId}")]
        public async Task<ActionResult> UpdateShipperById(string shipId, ShipperDto shipper, Boolean imgUpdate)
        {
            try
            {
                var ShipperToUpdate = await repository.Shipper.UpdateShipper(shipId, shipper, imgUpdate);
                var shipp = await repository.Shipper.GetShipperById(shipId);
                return Ok(shipp);
            }
            catch
            {
                return Ok(new
                {
                    message = "Hiện tại shipper đang có đơn hàng đi giao !!" +
                                               "Vui lòng xóa kiểm tra và thử lại "
                });
            }
        }
        /// <summary>
        /// Update status shipper  with pagination
        /// </summary>
        //PUT: api/v1/shipper?id
        [HttpPut("status/{shipId}")]
        public async Task<ActionResult> UpdateStatusStoreById(string shipId, StatusShipDto store)
        {
            try
            {
                var shipperToUpdate = await repository.Shipper.UpdateStatusShipper(shipId, store);
                var shipper = await repository.Shipper.GetShipperById(shipId);
                return Ok(new { StatusCode = "Successful", data = shipper });
            }
            catch
            {
                return Ok(new
                {
                    StatusCode = "Fail",
                    message = "Hiện tại đang có đơn hàng chưa hoàn thành !!" +
                                              "Vui lòng kiểm tra lại đơn hàng và thử lại "
                });
            }
        }
    }
}
