using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DeliveryVHGP_WebApi.Models;
using DeliveryVHGP_WebApi.IRepositories;
using DeliveryVHGP_WebApi.ViewModels;

namespace DeliveryVHGP_WebApi.Controllers
{
    [Route("api/v1/shipper-management/shippers")]
    [ApiController]
    public class ShippersManagementController : ControllerBase
    {
        private readonly IShipperRepository _shipperRepository;

        public ShippersManagementController(IShipperRepository shipperRepository)
        {
            _shipperRepository = shipperRepository;
        }
        /// <summary>
        /// Get list all shipper with pagination
        /// </summary>
        //GET: api/v1/shipper?pageIndex=1&pageSize=3
        [HttpGet]
        public async Task<ActionResult> GetAll(int pageIndex, int pageSize)
        {
            return Ok(await _shipperRepository.GetListShipper(pageIndex, pageSize));
        }
        /// <summary>
        /// Get Shipper by id with pagination
        /// </summary>
        //GET: api/v1/Shipper?pageIndex=1&pageSize=3
        [HttpGet("ByShipId")]
        public async Task<ActionResult<ShipperModel>> GetCategoryById(string id)
        {
            var ship = await _shipperRepository.GetShipperById(id);
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
                var result = await _shipperRepository.CreateShipper(shipper);
                return Ok(result);
            }
            catch
            {
                return Conflict();
            }
        }
    }
}
