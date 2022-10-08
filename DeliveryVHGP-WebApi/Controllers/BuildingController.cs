using DeliveryVHGP_WebApi.IRepositories;
using DeliveryVHGP_WebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryVHGP_WebApi.Controllers
{
    [Route("api/v1/buildings")]
    [ApiController]
    public class BuildingController : ControllerBase
    {
        private readonly IBuildingRepository _buildingRepository;

        public BuildingController(IBuildingRepository buildingRepository)
        {
            _buildingRepository = buildingRepository;
        }
        /// <summary>
        /// Get list all Bulding with pagination
        /// </summary>
        //GET: api/v1/Brand?pageIndex=1&pageSize=3
        [HttpGet]
        public async Task<ActionResult> GetAll(int pageIndex, int pageSize)
        {
            return Ok(await _buildingRepository.GetAll(pageIndex, pageSize));
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
                var result = await _buildingRepository.CreateBuilding(building);
                return Ok(result);
            }
            catch
            {
                return Conflict();
            }
        }
    }
}
