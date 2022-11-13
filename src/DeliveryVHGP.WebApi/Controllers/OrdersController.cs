using Microsoft.AspNetCore.Mvc;
using DeliveryVHGP.Core.Models;
using DeliveryVHGP.Core.Interfaces;

namespace DeliveryVHGP.WebApi.Controllers
{
    [Route("api/v1/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IRepositoryWrapper repository;
        public OrdersController(IRepositoryWrapper repository)
        {
            this.repository = repository;
        }
        /// <summary>
        /// Get list orders (customer web)
        /// </summary>
        // GET: api/Orders
        [HttpGet("{cusId}/customers")]
        public async Task<ActionResult> GetOrder(string cusId, int pageIndex, int pageSize)
        {
            var listOder = await repository.Order.GetListOrders(cusId, pageIndex, pageSize);
            if (cusId == null)
                return NotFound();
            return Ok(listOder);
        }
        /// <summary>
        /// Get list orders by store (customer web)
        /// </summary>
        // GET: api/Orders
        [HttpGet("stores/byStoreId")]
        public async Task<ActionResult> GetOrderByStore(string storeId, int pageIndex, int pageSize)
        {
            var listOder = await repository.Order.GetListOrdersByStore(storeId, pageIndex, pageSize);
            return Ok(listOder);
        }
        /// <summary>
        /// Get list orders by store (store web)
        /// </summary>
        // GET: api/Orders
        [HttpGet("stores/byStoreId/status/ByStatusId")]
        public async Task<ActionResult> GetOrderByStoreByStatus(int statusId ,string storeId, int pageIndex, int pageSize)
        {
            var listOder = await repository.Order.GetListOrdersByStoreByStatus(storeId, statusId, pageIndex, pageSize);
            if (storeId == null)
                return NotFound();
            return Ok(listOder);
        }
        /// <summary>
        /// Get order by id with pagination
        /// </summary>
        //GET: api/v1/orderById?pageIndex=1&pageSize=3
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
        /// Create a order (customer web)
        /// </summary>
        //POST: api/v1/order
        [HttpPost]
        public async Task<ActionResult> CreatNewOrder(OrderDto order)
        {
            try
            {
                var result = await repository.Order.CreatNewOrder(order);
                return Ok( new { StatusCode = "Successful" , data = result });
            }
            catch
            {
                return Ok(new
                {
                    StatusCode = "Fail", message = "Hiện tại cửa hàng đã ngưng hoạt động !!" +
                                              "Vui lòng đặt lại đơn hàng "
                });
            }
        }
        /// <remarks>
        /// Status in body
        /// CreateOrder = 1, ShipAccept = 2, Shipping = 3, Done = 4, Cancel = 5, ShipCancel = 6, ShipCancelOther = 7, ShopCancel = 8, CustomerCancel = 9, CustomerPayCancel = 10
        /// </remarks>
        /// <summary>
        /// Update a status order (customer web)
        /// </summary>
        //POST: api/v1/order
        [HttpPut("{orderId}")]
        public async Task<ActionResult<OrderStatusModel>> UpdateOrder(string orderId, OrderStatusModel order)
        {
            if (orderId != order.OrderId)
            {
                return BadRequest("Order Id mismatch");
            }
            if (order.OrderId == null)
            {
                return NotFound("Order Id does not exist");
            }
            try
            {
                await repository.Order.OrderUpdateStatus(orderId, order);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
            return Ok(order);
        }
        /// <summary>
        /// Get TimeDuration By MenuId in Mode3 (customer web)
        /// </summary>
        //GET: api/v1/order?pageIndex=1&pageSize=3 
        [HttpGet("ByMenuId")]
        public async Task<ActionResult> GetTimeDuration(string menuId, int pageIndex, int pageSize)
        {
            return Ok(await repository.Order.GetDurationOrder(menuId, pageIndex, pageSize));
        }
        /// <summary>
        /// Get order by id with pagination
        /// </summary>
        //GET: api/v1/orderById?pageIndex=1&pageSize=3
        [HttpGet("ByOrderId")]
        public async Task<ActionResult> GetPaymentOrder(string orderId)
        {
            try
            {
                var pro = await repository.Order.PaymentOrder(orderId);

                return Redirect(pro.ToString());
            }
            catch
            {
                return NotFound();
            }
        }

    }
}
