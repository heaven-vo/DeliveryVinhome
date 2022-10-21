using Microsoft.AspNetCore.Mvc;
using DeliveryVHGP.Core.Interfaces;
using DeliveryVHGP.Core.Models;

namespace DeliveryVHGP.WebApi.Controllers
{
    [Route("api/v1/collection-management/collections")]
    [ApiController]
    public class CollectionsManagementController : ControllerBase
    {
        private readonly IRepositoryWrapper repository;
        public CollectionsManagementController(IRepositoryWrapper repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Get list all collection with pagination
        /// </summary>
        //GET: api/v1/collection?pageIndex=1&pageSize=3
        [HttpGet]
        public async Task<ActionResult> GetAll(int pageIndex, int pageSize)
        {
            return Ok(await repository.Collection.GetAll(pageIndex, pageSize));
        }
        /// <summary>
        /// Create a collection
        /// </summary>
        //POST: api/v1/collection
        [HttpPost]
        public async Task<ActionResult> CreateCollection(CollectionModel collection)
        {
            try
            {
                var result = await repository.Collection.CreateCollection(collection);
                return Ok(result);
            }
            catch
            {
                return Conflict();
            }
        }
    }
}
