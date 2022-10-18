using DeliveryVHGP_WebApi.IRepositories;
using DeliveryVHGP_WebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryVHGP_WebApi.Controllers
{
    [Route("api/v1/storeCategory-management/storeCategories")]
    [ApiController]
    public class StoreCategoryManagementController : ControllerBase
    {
        private readonly IStoreCategoryRepository _storeCategoryRepository;

        public StoreCategoryManagementController(IStoreCategoryRepository storeCategoryRepository)
        {
            _storeCategoryRepository = storeCategoryRepository;
        }
        /// <summary>
        /// Get list all storeCategory with pagination
        /// </summary>
        //GET: api/v1/storeCategory?pageIndex=1&pageSize=3
        [HttpGet]
        public async Task<ActionResult> GetAll(int pageIndex, int pageSize)
        {
            return Ok(await _storeCategoryRepository.GetAll(pageIndex, pageSize));
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
                var result = await _storeCategoryRepository.CreateStoreCategory(storeCate);
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
                var result = await _storeCategoryRepository.DeleteById(id);
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
                var BrandToUpdate = await _storeCategoryRepository.UpdateStoreCateById(id, storeCate);
                return Ok(storeCate);
            }
            catch
            {
                return Conflict();
            }
        }

    }
}
