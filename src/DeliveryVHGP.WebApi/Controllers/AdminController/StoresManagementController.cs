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
        public async Task<ActionResult> GetAll(int pageIndex, int pageSize, [FromQuery] FilterRequestInStore request)
        {
            return Ok(await repository.Store.GetListStore(pageIndex, pageSize, request));
        }
        /// <summary>
        /// Get order report in store
        /// </summary>
        //GET: api/v1/orderReportBystore?pageIndex=1&pageSize=3
        [HttpGet("orderReport")]
        public async Task<ActionResult> GetReportOrderInStore(string storeId, [FromQuery] DateFilterRequest request, [FromQuery] MonthFilterRequest monthFilter)
        {
            return Ok(await repository.Store.GetListOrdersReport(storeId, request, monthFilter));
        }
        /// <summary>
        /// Get order report Revenue in store
        /// </summary>
        //GET: api/v1/orderReportRevenueBystore?pageIndex=1&pageSize=3
        [HttpGet("orderReport-Revenue")]
        public async Task<ActionResult> GetReportOrderRevenueInStore(string storeId, [FromQuery] DateFilterRequest request, [FromQuery] MonthFilterRequest monthFilter)
        {
            return Ok(await repository.Store.GetPriceOrdersReports(storeId, request, monthFilter));
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
            try
            {
                var store = await repository.Store.GetStoreById(storeId);
                if (storeId == null)
                    return NotFound();
                return Ok(store);
            }
            catch (Exception e)
            {
                return Ok(new
                {
                    StatusCode = "Fail",
                    message = e.Message
                });
            }
        }
        /// <summary>
        /// Get list orders preparing by store ,status:3 (store app) 
        /// </summary>
        // GET: api/Orders
        [HttpGet("byStoreId/orders-preparing")]
        public async Task<ActionResult> GetOrderPreparingByStore(string storeId, int pageIndex, int pageSize)
        {
            var listOder = await repository.Store.GetListOrderPreparingsByStore(storeId, pageIndex, pageSize);
            if (storeId == null)
                return NotFound();
            return Ok(listOder);
        }
        /// <summary>
        /// Get list orders delivering by store ,status:4,7,8 (store app) 
        /// </summary>
        // GET: api/Orders
        [HttpGet("byStoreId/orders-delivering")]
        public async Task<ActionResult> GetOrderDeliveringByStore(string storeId, int pageIndex, int pageSize)
        {
            var listOder = await repository.Store.GetListOrderDeliveringByStore(storeId, pageIndex, pageSize);
            if (storeId == null)
                return NotFound();
            return Ok(listOder);
        }
        /// <summary>
        /// Get list orders completed by store ,status:5,6,9,10,11,12 (store app) 
        /// </summary>
        // GET: api/Orders
        [HttpGet("byStoreId/orders-completed")]
        public async Task<ActionResult> GetOrderCompletedByStore(string storeId, int pageIndex, int pageSize, [FromQuery] FilterRequest request)
        {
            var listOder = await repository.Store.GetListOrderCompletedByStore(storeId, pageIndex, pageSize, request);
            if (storeId == null)
                return NotFound();
            return Ok(listOder);
        }
        /// <summary>
        /// Get list orders byMode in store ,status:2,3 (store app) 
        /// </summary>
        // GET: api/Orders
        [HttpGet("byStoreId/byModeId/order")]
        public async Task<ActionResult> GetOrderByStoreByMode(string storeId, string modeId, [FromQuery] DateFilterRequest request, int pageIndex, int pageSize)
        {
            var listOder = await repository.Store.GetListOrderByStoreByModeId(storeId, modeId, request, pageIndex, pageSize);
            if (storeId == null)
                return NotFound();
            return Ok(listOder);
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
        public async Task<ActionResult> UpdateStoreById(string storeId, StoreDto store, Boolean imgUpdate)
        {
            try
            {
                var productToUpdate = await repository.Store.UpdateStore(storeId, store, imgUpdate);
                var storee = await repository.Store.GetStoreById(storeId);
                return Ok(new { StatusCode = "Successful", data = storee });
            }
            catch (Exception e)
            {
                return Ok(new
                {
                    StatusCode = "Fail",
                    message = e.Message
                });
            }
        }
        /// <summary>
        /// Update status store  with pagination
        /// </summary>
        //PUT: api/v1/store?id
        [HttpPut("status/{storeId}")]
        public async Task<ActionResult> UpdateStatusStoreById(string storeId, StatusStoreDto store)
        {
            try
            {
                var productToUpdate = await repository.Store.UpdateStatusStore(storeId, store);
                var storee = await repository.Store.GetStoreById(storeId);
                return Ok(new { StatusCode = "Successful", data = storee });
            }
            catch (Exception e)
            {
                return Ok(new
                {
                    StatusCode = "Fail",
                    message = e.Message
                });
            }
        }
        [HttpGet("storeId-wallets")]
        public async Task<ActionResult> AddWallerbyStore(string storeId)
        {
            var store = await repository.Store.CreatWallet(storeId);
            if (storeId == null)
                return NotFound();
            return Ok(store);
        }
        [HttpGet("{storeId}/wallet")]
        public async Task<ActionResult<WalletsStoreModel>> GetBalaceStoreWallet(string storeId)
        {
            try
            {
                var walletBalance = await repository.Store.GetBalanceStoreWallet(storeId);
                return Ok(new { StatusCode = "Successful", data = walletBalance });
            }
            catch (Exception e)
            {
                return Ok(new
                {
                    StatusCode = "Fail",
                    message = e.Message
                });
            }
        }
    }
}
