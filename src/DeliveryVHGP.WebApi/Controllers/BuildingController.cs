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
        /// Get Category by id with pagination
        /// </summary>
        //GET: api/v1/Category?pageIndex=1&pageSize=3
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryModel>> GetBuildingById(string id)
        {
            var building = await repository.Building.GetBuildinById(id);
            if (building == null)
                return NotFound();
            return Ok(building);
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
        /// <summary>
        /// Update Long,Lat Building  with pagination
        /// </summary>
        //PUT: api/v1/Building?id
        [HttpPut("ByBuildingId")]
        public async Task<ActionResult> UpdateLongLatBuilding(string buildingId, BuildingDto building)
        {
            try
            {
                var BuildingtoUpdate = await repository.Building.UpdateLongLatBuilding(buildingId, building);
                var buildingg = await repository.Building.GetBuildinById(buildingId);
                return Ok(new { StatusCode = "Successful", data = buildingg });
            }
            catch
            {
                return Conflict(); ;
            }
        }
    }
}
