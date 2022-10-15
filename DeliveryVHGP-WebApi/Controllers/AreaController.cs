using DeliveryVHGP_WebApi.IRepositories;
using DeliveryVHGP_WebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryVHGP_WebApi.Controllers
{
    [Route("api/v1/areas")]
    [ApiController]
    public class AreaController : ControllerBase
    {
        private readonly IAreaRepository _areaRepository;

        public AreaController(IAreaRepository areaRepository)
        {
            _areaRepository = areaRepository;
        }
        /// <summary>
        /// Get list all Bulding with pagination
        /// </summary>
        //GET: api/v1/Brand?pageIndex=1&pageSize=3
        [HttpGet]
        public async Task<ActionResult> GetAll(int pageIndex, int pageSize)
        {
            return Ok(await _areaRepository.GetAll(pageIndex, pageSize));
        }
        /// <summary>
        /// Get building by Areaid with pagination
        /// </summary>
        //GET: api/v1/buildingbyAreaId?pageIndex=1&pageSize=3
        [HttpGet("ByAreaId")]
        public async Task<ActionResult> GetBuildingByAreaId(string areaId)
        {
            var building = await _areaRepository.GetBuildingByArea(areaId);
            if (building == null)
                return NotFound();
            return Ok(building);
        }
        /// <summary>
        /// Create a Area 
        /// </summary>
        //POST: api/v1/area
        [HttpPost]
        public async Task<ActionResult> CreateArea(AreaModel area)
        {
            try
            {
                var result = await _areaRepository.CreateArea(area);
                return Ok(result);
            }
            catch
            {
                return Conflict();
            }
        }
        /// <summary>
        /// Delete a area by id
        /// </summary>
        //DELETE: api/v1/area/{id}
        [HttpDelete("{areaId}")]
        public async Task<ActionResult> DeleteArea(string areaId)
        {
            try
            {
                var result = await _areaRepository.DeleteById(areaId);
                return Ok(result);
            }
            catch
            {
                return Conflict();
            }

        }
        /// <summary>
        /// Update Area with pagination
        /// </summary>
        //PUT: api/v1/Area?id
        [HttpPut("{areaId}")]
        public async Task<ActionResult> UpdateArea(string areaId, AreaDto area)
        {
            try
            {
                if (areaId != area.Id)
                {
                    return BadRequest("Area ID mismatch");
                }
                var BrandToUpdate = await _areaRepository.UpdateAreaById(areaId, area);
                return Ok(area);
            }
            catch
            {
                return Conflict();
            }
        }
    }
}
