using DeliveryVHGP.Core.Interfaces;
using DeliveryVHGP.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryVHGP.WebApi.Controllers
{
    [Route("api/v1/storeCategories")]
    [ApiController]
    public class StoreCategoryController : ControllerBase
    {
        private readonly IRepositoryWrapper repository;
        public StoreCategoryController(IRepositoryWrapper repository)
        {
            this.repository = repository;
        }
        /// <summary>
        /// Get list all storeCategory with pagination
        /// </summary>
        //GET: api/v1/storeCategory?pageIndex=1&pageSize=3
        [HttpGet]
        public async Task<ActionResult> GetAll(int pageIndex, int pageSize)
        {
            return Ok(await repository.StoreCategory.GetAll(pageIndex, pageSize));
        }
        /// <summary>
        /// Create a storeCategory
        /// </summary>
        //POST: api/v1/storeCategory
        [HttpPost]
        public async Task<ActionResult> CreateStoreCategory(StoreCategoryDto storeCate)
        {
            try
            {
                var result = await repository.StoreCategory.CreateStoreCategory(storeCate);
                return Ok(result);
            }
            catch
            {
                return Conflict();
            }


        }

    }
}
