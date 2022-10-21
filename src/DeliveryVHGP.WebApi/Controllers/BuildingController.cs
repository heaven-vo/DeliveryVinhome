using DeliveryVHGP.Core.Interfaces;
using DeliveryVHGP.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryVHGP.WebApi.Controllers
{
    [Route("api/v1/buildings")]
    [ApiController]
    public class BuildingController : ControllerBase
    {
        private readonly IRepositoryWrapper repository;
        public BuildingController(IRepositoryWrapper repository)
        {
            this.repository = repository;
        }
        /// <summary>
        /// Get list all Bulding with pagination
        /// </summary>
        //GET: api/v1/Brand?pageIndex=1&pageSize=3
        [HttpGet]
        public async Task<ActionResult> GetAll(int pageIndex, int pageSize)
        {
            return Ok(await repository.Building.GetAll(pageIndex, pageSize));
        }
        /// <summary>
        /// Create a Building (customer web)
        /// </summary>
        //POST: api/v1/building
        [HttpPost]
        public async Task<ActionResult> CreateBuilding(BuildingModel building)
        {
            try
            {
                var result = await repository.Building.CreateBuilding(building);
                return Ok(result);
            }
            catch
            {
                return Conflict();
            }
        }
    }
}
