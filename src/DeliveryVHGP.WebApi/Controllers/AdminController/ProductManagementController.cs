using Microsoft.AspNetCore.Mvc;
using DeliveryVHGP.Core.Interfaces;
using DeliveryVHGP.Core.Models;

namespace DeliveryVHGP.WebApi.Controllers
{
    [Route("api/v1/product-management/products")]
    [ApiController]
        public class ProductManagementController : ControllerBase
    {
        private readonly IRepositoryWrapper repository;
        public ProductManagementController(IRepositoryWrapper repository)
        {
            this.repository = repository;
        }
        /// <summary>
        /// Get list product in menu  with pagination (web for admin)
        /// </summary>
        //GET: api/v1/productbyMenuId?pageIndex=1&pageSize=3
        [HttpGet("{menuId}/products")]
        public async Task<ActionResult> GetListProduct(string menuId,int pageIndex, int pageSize)
        {
            var pro = await repository.Product.GetListProduct(menuId,pageIndex, pageSize);
            if (pro == null)
                return NotFound();
            return Ok(pro);
        }
       
    }
}
