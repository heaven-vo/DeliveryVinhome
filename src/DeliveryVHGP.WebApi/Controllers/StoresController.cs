using Microsoft.AspNetCore.Mvc;
using DeliveryVHGP.Core.Interfaces;
using static DeliveryVHGP.Core.Models.OrderAdminDto;
using DeliveryVHGP.Core.Models;

namespace DeliveryVHGP.WebApi.Controllers
{
    [Route("api/v1/stores")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private readonly IRepositoryWrapper repository;
        public StoresController(IRepositoryWrapper repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Get list all store with pagination
        /// </summary>
        //GET: api/v1/store?pageIndex=1&pageSize=3 
        [HttpGet]
        public async Task<ActionResult> GetAll( int pageIndex, int pageSize, [FromQuery] FilterRequestInStore request)
        {
            return Ok(await repository.Store.GetListStore( pageIndex, pageSize, request));
        }
       
        /// <summary>
        /// Get list all store by brand with pagination
        /// </summary>
        //GET: api/v1/storeByBrand?pageIndex=1&pageSize=3
        [HttpGet("search/{brandName}")]
        public async Task<ActionResult> GetListStoreByBrand( string brandName, int pageIndex, int pageSize)
        {
            return Ok(await repository.Store.GetListStoreInBrand(brandName, pageIndex, pageSize));
        } 
        /// <summary>
        /// Get list all store by brand with pagination
        /// </summary>
        //GET: api/v1/storeByBrand?pageIndex=1&pageSize=3
        [HttpGet("search/{storeName}")]
        public async Task<ActionResult> GetListStoreByName( string storeName, int pageIndex, int pageSize)
        {
            return Ok(await repository.Store.GetListStoreByName(storeName, pageIndex, pageSize));
        }
    }

}
