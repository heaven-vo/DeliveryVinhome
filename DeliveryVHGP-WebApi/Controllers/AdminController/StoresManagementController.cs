using Microsoft.AspNetCore.Mvc;
using DeliveryVHGP_WebApi.IRepositories;
using DeliveryVHGP_WebApi.ViewModels;
using DeliveryVHGP_WebApi.Models;

namespace DeliveryVHGP_WebApi.Controllers
{
    [Route("api/v1/store-management")]
    [ApiController]
    public class StoresManagementController : ControllerBase
    {
        private readonly IStoreRepository _storeRepository;
        public StoresManagementController(IStoreRepository storeRepository) { 
            _storeRepository = storeRepository;
        }
        /// <summary>
        /// Get list all store with pagination
        /// </summary>
        //GET: api/v1/store?pageIndex=1&pageSize=3
        [HttpGet]
        public async Task<ActionResult> GetAll( int pageIndex, int pageSize)
        {
            return Ok(await _storeRepository.GetListStore( pageIndex, pageSize));
        }
        /// <summary>
        /// Get list all store by brand with pagination
        /// </summary>
        //GET: api/v1/storeByBrand?pageIndex=1&pageSize=3
        [HttpGet("store/brand/{brandName}")]
        public async Task<ActionResult> GetListStoreByBrand(string brandName, int pageIndex, int pageSize)
        {
            return Ok(await _storeRepository.GetListStoreInBrand(brandName, pageIndex, pageSize));
        } /// <summary>
        /// Get list all store by brand with pagination
        /// </summary>
        //GET: api/v1/storeByBrand?pageIndex=1&pageSize=3
        [HttpGet("store/{name}")]
        public async Task<ActionResult> GetListStoreByName( string name, int pageIndex, int pageSize)
        {
            return Ok(await _storeRepository.GetListStoreByName(name, pageIndex, pageSize));
        }

        /// <summary>
        /// Create a product
        /// </summary>
        //POST: api/v1/product
        [HttpPost]
        public async Task<ActionResult> CreateProduct(ProductModel product)
        {
            try
            {
                var result = await _storeRepository.CreatNewProduct(product);
                return Ok(result);
            }
            catch
            {
                return Conflict();
            }
        }
    }
}
