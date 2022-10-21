using Microsoft.AspNetCore.Mvc;
using DeliveryVHGP.Core.Interfaces;
using DeliveryVHGP.Core.Models;

namespace DeliveryVHGP.WebApi.Controllers
{
    [Route("api/v1/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IRepositoryWrapper repository;

        public CategoriesController(IRepositoryWrapper repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Get list all category with pagination
        /// </summary>
        //GET: api/v1/category?pageIndex=1&pageSize=3
        [HttpGet]
        public async Task<ActionResult> GetAll(int pageIndex, int pageSize)
        {
            return Ok(await repository.Category.GetAll(pageIndex, pageSize));
        }

        /// <summary>
        /// Get list Category in a menu
        /// </summary>
        [HttpGet("menus")]
        public async Task<ActionResult<List<ProductViewInList>>> GetAllProductInMenu(string menuId, int page, int pageSize)
        {
            return Ok( await repository.Category.GetListCategoryByMenuId(menuId, page, pageSize));
            
        }

     
    }
}
