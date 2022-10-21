using DeliveryVHGP.Core.Interfaces;
using DeliveryVHGP.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryVHGP.WebApi.Controllers
{
    [Route("api/v1/storeCategory-management/storeCategories")]
    [ApiController]
    public class StoreCategoryManagementController : ControllerBase
    {
        private readonly IRepositoryWrapper repository;
        public StoreCategoryManagementController(IRepositoryWrapper repository)
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
        /// <summary>
        /// Delete a storeCategory by id
        /// </summary>
        //DELETE: api/v1/storeCategory/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStoreCategory(string id)
        {
            try
            {
                var result = await repository.StoreCategory.DeleteById(id);
                return Ok(result);
            }
            catch
            {
                return Conflict();
            }

        }
        /// <summary>
        /// Update storeCategory with pagination
        /// </summary>
        //PUT: api/v1/storeCategory?id
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateStoreCategory(string id, StoreCategoryModel storeCate)
        {
            try
            {
                if (id != storeCate.Id)
                {
                    return BadRequest("StoreCategory ID mismatch");
                }
                var BrandToUpdate = await repository.StoreCategory.UpdateStoreCateById(id, storeCate);
                return Ok(storeCate);
            }
            catch
            {
                return Conflict();
            }
        }

    }
}
