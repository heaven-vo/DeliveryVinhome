using Microsoft.AspNetCore.Mvc;
using DeliveryVHGP.Core.Interfaces;

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
        public async Task<ActionResult> GetOrder(int pageIndex, int pageSize)
        {
            try
            {
                var result = Ok(await repository.Order.GetAll(pageIndex, pageSize));

                return Ok(result);
            }
            catch
            {
                return Conflict();
            }
        }
        /// <summary>
        /// Get list all order by payment with pagination
        /// </summary>
        //GET: api/v1/OrderByPayment?pageIndex=1&pageSize=3
        [HttpGet("search-payment")]
        public async Task<ActionResult> GetListOrderByPayment(string paymentType, int pageIndex, int pageSize)
        {
            return Ok(await repository.Order.GetOrderByPayment(paymentType, pageIndex, pageSize));
        }
        /// <summary>
        /// Get list all order by status with pagination
        /// </summary>
        //GET: api/v1/orderByStatus?pageIndex=1&pageSize=3
        [HttpGet("search-status")]
        public async Task<ActionResult> GetListOrderByStatus(string statusName, int pageIndex, int pageSize)
        {
            return Ok(await repository.Order.GetOrderByStatus(statusName, pageIndex, pageSize));
        }
        /// <summary>
        /// Get product by id with pagination
        /// </summary>
        //GET: api/v1/productbyId?pageIndex=1&pageSize=3
        [HttpGet("{id}")]
        public async Task<ActionResult> GetOrderDetail(string id)
        {
            var pro = await repository.Order.GetOrdersById(id);
            if (pro == null)
                return NotFound();
            return Ok(pro);
        }
    }
}
