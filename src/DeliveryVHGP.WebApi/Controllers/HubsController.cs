using Microsoft.AspNetCore.Mvc;
using DeliveryVHGP.Core.Interfaces;
using DeliveryVHGP.Core.Models;

namespace DeliveryVHGP.WebApi.Controllers
{
    [Route("api/v2/hubs")]
    [ApiController]
    public class HubsController : ControllerBase
    {
        private readonly IRepositoryWrapper repository;
        public HubsController(IRepositoryWrapper repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Get list all hub with pagination
        /// </summary>
        //GET: api/v1/Hub?pageIndex=1&pageSize=3
        [HttpGet]
        public async Task<ActionResult> GetAll(int pageIndex, int pageSize, [FromQuery] FilterRequestInHub request)
        {
            return Ok(await repository.Hub.GetlistHub(pageIndex, pageSize, request));
        }

        /// <summary>
        /// Get a hub by id
        /// </summary>
        //GET: api/v1/hub/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(string id)
        {
            var hub = await repository.Hub.GetById(id);
            if (hub == null)
                return NotFound();
            return Ok(hub);
        }
        /// <summary>
        /// Create a hub
        /// </summary>
        //POST: api/v1/hub
        [HttpPost]
        public async Task<ActionResult> CreateHub(HubDto hub)
        {
            try
            {
                var result = await repository.Hub.CreateHub(hub);
                return Ok(result);
            }
            catch
            {
                return Conflict();
            }
        }
        /// <summary>
        /// Delete a hub by id
        /// </summary>
        //DELETE: api/v1/hub/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteHub(string id)
        {
            try
            {
                var result = await repository.Hub.DeleteById(id);
                return Ok(result);
            }
            catch
            {
                return Conflict();
            }

        }
        /// <summary>
        /// Update Hub with pagination
        /// </summary>
        //PUT: api/v1/Hub?id
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateHub(string id, HubModels hub)
        {
            try
            {
                if (id != hub.Id)
                {
                    return BadRequest("Hub ID mismatch");
                }
                var HubToUpdate = await repository.Hub.UpdateHubById(id, hub);
                return Ok(hub);
            }
            catch
            {
                return Conflict();
            }
        }
    }
}