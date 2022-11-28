using DeliveryVHGP.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static DeliveryVHGP.Core.Models.OrderAdminDto;

namespace DeliveryVHGP.WebApi.Controllers
{
    [Route("api/v1/order-management/orders")]
    [ApiController]
    public class OrdersManagementController : ControllerBase
    {
        private readonly IRepositoryWrapper repository;
        public OrdersManagementController(IRepositoryWrapper repository)
        {
            this.repository = repository;
        }
        /// <summary>
        /// Get list order with pagination
        /// </summary>
        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult> GetOrder(int pageIndex, int pageSize, [FromQuery] FilterRequest request)
        {
            try
            {
                var pro = await repository.Order.GetAll(pageIndex, pageSize, request);
                int total = pro.Count;
                return Ok(new { TotalOrder = total, data = pro });

            }
            catch
            {
                return Conflict();
            }
        }
        /// <summary>
        /// Get order report(admin web)
        /// </summary>
        //GET: api/v1/OrderReport?pageIndex=1&pageSize=3
        [HttpGet("report")]
        public async Task<ActionResult> GetListOrdersReport([FromQuery] DateFilterRequest request)
        {
            return Ok(await repository.Order.GetListOrdersReport(request));
        }
        /// <summary>
        /// Get list all order by payment with pagination
        /// </summary>
        //GET: api/v1/OrderByPayment?pageIndex=1&pageSize=3
        [HttpGet("search-payment")]
        public async Task<ActionResult> GetListOrderByPayment(int paymentType, int pageIndex, int pageSize)
        {
            return Ok(await repository.Order.GetOrderByPayment(paymentType, pageIndex, pageSize));
        }
        /// <summary>
        /// Get list all order by status with pagination
        /// </summary>
        //GET: api/v1/orderByStatus?pageIndex=1&pageSize=3
        [HttpGet("search-status")]
        public async Task<ActionResult> GetListOrderByStatus(int status, int pageIndex, int pageSize)
        {
            return Ok(await repository.Order.GetOrderByStatus(status, pageIndex, pageSize));
        }
        /// <summary>
        /// Get product by id with pagination
        /// </summary>
        //GET: api/v1/productbyId?pageIndex=1&pageSize=3
        [HttpGet("{id}")]
        public async Task<ActionResult> GetOrderDetail(string id)
        {
            try
            {
                var pro = await repository.Order.GetOrdersById(id);
                return Ok(pro);
            }
            catch
            {
                return NotFound();
            }
        }
        /// <summary>
        /// Clear order 
        /// </summary>
        //DELETE: api/v1/buildingg/{id}
        [HttpDelete]
        public async Task<ActionResult> DeleteOrder()
        {
            try
            {
                await repository.Order.DeleteOrder();
                return Ok(new { StatusCode = "Delete Successful" });
            }
            catch
            {
                return NotFound(); ;
            }
        }
    }
}
