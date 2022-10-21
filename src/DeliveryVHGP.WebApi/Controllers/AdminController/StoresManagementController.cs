using DeliveryVHGP.Core.Interfaces;
using DeliveryVHGP.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryVHGP.WebApi.Controllers
{
    [Route("api/v1/store-management/stores")]
    [ApiController]
    public class StoresManagementController : ControllerBase
    {
        private readonly IRepositoryWrapper repository;

        public StoresManagementController(IRepositoryWrapper repository)
        {
            this.repository = repository;
        }
        /// <summary>
        /// Get list all store with pagination
        /// </summary>
        //GET: api/v1/store?pageIndex=1&pageSize=3
        [HttpGet]
        public async Task<ActionResult> GetAll(int pageIndex, int pageSize)
        {
            return Ok(await repository.Store.GetListStore(pageIndex, pageSize));
        }
        /// <summary>
        /// Get list all store by brand with pagination
        /// </summary>
        //GET: api/v1/storeByBrand?pageIndex=1&pageSize=3
        [HttpGet("search-brand")]
        public async Task<ActionResult> GetListStoreByBrand(string brandName, int pageIndex, int pageSize)
        {
            return Ok(await repository.Store.GetListStoreInBrand(brandName, pageIndex, pageSize));
        } /// <summary>
          /// Get list all store by brand with pagination
          /// </summary>
        //GET: api/v1/storeByBrand?pageIndex=1&pageSize=3
        [HttpGet("search-name")]
        public async Task<ActionResult> GetListStoreByName(string storeName, int pageIndex, int pageSize)
        {
            return Ok(await repository.Store.GetListStoreByName(storeName, pageIndex, pageSize));
        }
        /// <summary>
        /// Get store by id with pagination
        /// </summary>
        //GET: api/v1/storeById?pageIndex=1&pageSize=3
        [HttpGet("storeId")]
        public async Task<ActionResult> GetStoreById(string storeId)
        {
            var store = await repository.Store.GetStoreById(storeId);
            if (storeId == null)
                return NotFound();
            return Ok(store);
        }
        /// <summary>
        /// Create a new store
        /// </summary>
        //POST: api/v1/store
        [HttpPost]
        public async Task<ActionResult> CreateNewStore(StoreDto storeId)
        {
            try
            {
                var result = await repository.Store.CreatNewStore(storeId);
                return Ok(result);
            }
            catch
            {
                return Conflict();
            }
        }
        /// <summary>
        /// Delete for Store
        /// </summary>
        //POST: api/v1/store
        [HttpDelete("{storeId}")]
        public async Task<IActionResult> DeleteStore(string storeId)
        {
            try
            {
                var store = await repository.Store.DeleteStore(storeId);
                return Ok(store);
            }
            catch (Exception)
            {
                return Ok(new
                {
                    message = "Hiện tại cửa hàng đang có trong menu !!" +
                                              "Vui lòng xóa cửa hàng khỏi menu và thử lại "
                });
            }
        }
        /// <summary>
        /// Update status store  with pagination
        /// </summary>
        //PUT: api/v1/store?id
        [HttpPut("{storeId}")]
        public async Task<ActionResult> UpdateProById(string storeId, StoreDto store , Boolean imgUpdate)
        {
            try
            {
                var productToUpdate = await repository.Store.UpdateStore(storeId, store, imgUpdate);
                var storee = await repository.Store.GetStoreById(storeId);
                return Ok(storee);
            }
            catch
            {
                return Conflict();
            }
        }
    }
}
