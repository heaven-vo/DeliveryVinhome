using DeliveryVHGP_WebApi.IRepositories;
using DeliveryVHGP_WebApi.ViewModels;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryVHGP_WebApi.Controllers
{
    [Route("api/v1/store-management/stores")]
    [ApiController]
    public class StoresManagementController : ControllerBase
    {
        private readonly IStoreRepository _storeRepository;

  
        public StoresManagementController(IStoreRepository storeRepository)
        {
            _storeRepository = storeRepository;
        }
        /// <summary>
        /// Get list all store with pagination
        /// </summary>
        //GET: api/v1/store?pageIndex=1&pageSize=3
        [HttpGet]
        public async Task<ActionResult> GetAll(int pageIndex, int pageSize)
        {
            return Ok(await _storeRepository.GetListStore(pageIndex, pageSize));
        }
        /// <summary>
        /// Get list all store by brand with pagination
        /// </summary>
        //GET: api/v1/storeByBrand?pageIndex=1&pageSize=3
        [HttpGet("search-brand")]
        public async Task<ActionResult> GetListStoreByBrand(string brandName, int pageIndex, int pageSize)
        {
            return Ok(await _storeRepository.GetListStoreInBrand(brandName, pageIndex, pageSize));
        } /// <summary>
          /// Get list all store by brand with pagination
          /// </summary>
        //GET: api/v1/storeByBrand?pageIndex=1&pageSize=3
        [HttpGet("search-name")]
        public async Task<ActionResult> GetListStoreByName(string storeName, int pageIndex, int pageSize)
        {
            return Ok(await _storeRepository.GetListStoreByName(storeName, pageIndex, pageSize));
        }
        /// <summary>
        /// Get store by id with pagination
        /// </summary>
        //GET: api/v1/storeById?pageIndex=1&pageSize=3
        [HttpGet("storeId")]
        public async Task<ActionResult> GetStoreById(string storeId)
        {
            var store = await _storeRepository.GetStoreById(storeId);
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
                var result = await _storeRepository.CreatNewStore(storeId);
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
                var store = await _storeRepository.DeleteStore(storeId);
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
        public async Task<ActionResult> UpdateProById(string storeId, StoreDto store)
        {
            try
            {
                var productToUpdate = await _storeRepository.UpdateStore(storeId, store);
                return Ok(storeId);
            }
            catch
            {
                return Conflict();
            }
        }
    }
}
