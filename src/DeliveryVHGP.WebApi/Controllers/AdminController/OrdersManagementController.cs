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

        // GET: api/Orders
        [HttpGet("{CusId}/Customer")]
        public async Task<ActionResult> GetOrder(string CusId,int pageIndex, int pageSize)
        {
            try
            {
                var result = Ok(await repository.Order.GetListOrders(CusId,pageIndex, pageSize));

                return Ok(result);
            }
            catch
            {
                return Conflict();
            }
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
