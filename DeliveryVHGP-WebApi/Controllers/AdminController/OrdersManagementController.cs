using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DeliveryVHGP_WebApi.Models;
using DeliveryVHGP_WebApi.IRepositories;
using DeliveryVHGP_WebApi.ViewModels;

namespace DeliveryVHGP_WebApi.Controllers
{
    [Route("api/v1/order-management/orders")]
    [ApiController]
    public class OrdersManagementController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrdersManagementController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // GET: api/Orders
        [HttpGet("{CusId}/Customer")]
        public async Task<ActionResult> GetOrder(string CusId,int pageIndex, int pageSize)
        {
            try
            {
                var result = Ok(await _orderRepository.GetListOrders(CusId,pageIndex, pageSize));
            
        //        return Ok(result);
        //    }
        //    catch
        //    {
        //        return Conflict();
        //    }
        //}
        /// <summary>
        /// Get product by id with pagination
        /// </summary>
        //GET: api/v1/productbyId?pageIndex=1&pageSize=3
        [HttpGet("{id}")]
        public async Task<ActionResult> GetOrderDetail(string id)
        {
            var pro = await _orderRepository.GetOrdersById(id);
            if (pro == null)
                return NotFound();
            return Ok(pro);
        }
    }
}
