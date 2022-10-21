using Microsoft.AspNetCore.Mvc;
using DeliveryVHGP.Core.Interfaces;
using DeliveryVHGP.Core.Models;

namespace DeliveryVHGP.WebApi.Controllers
{
    [Route("api/v1/category-management/categories")]
    [ApiController]
    public class CategoriesManagementController : ControllerBase
    {
        private readonly IRepositoryWrapper repository;

        public CategoriesManagementController(IRepositoryWrapper repository)
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
        /// Get Category by id with pagination
        /// </summary>
        //GET: api/v1/Category?pageIndex=1&pageSize=3
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryModel>> GetCategoryById(string id)
        {
            var cate = await repository.Category.GetCategoryById(id);
            if (cate == null)
                return NotFound();
            return Ok(cate);
        }
        /// <summary>
        /// Get list Category in a menu
        /// </summary>
        [HttpGet("menus")]
        public async Task<ActionResult<List<ProductViewInList>>> GetAllProductInMenu(string menuId, int page, int pageSize)
        {
            return Ok(await repository.Category.GetListCategoryByMenuId(menuId, page, pageSize));
        }
        /// <summary>
        /// Get list all category by name with pagination
        /// </summary>
        //GET: api/v1/cateByName?pageIndex=1&pageSize=3
        [HttpGet("search-name")]
        public async Task<ActionResult> GetListStoreByName(string cateName, int pageIndex, int pageSize)
        {
            return Ok(await repository.Category.GetListCategoryByName(cateName, pageIndex, pageSize));
        }
        /// <summary>
        /// Create a category
        /// </summary>
        //POST: api/v1/category
        [HttpPost]
        public async Task<ActionResult> CreateCategory(CategoryDto category)
        {
            try
            {
                var result = await repository.Category.CreateCategory(category);
                return Ok(result);
            }
            catch
            {
                return Conflict();
            }
        }
        /// <summary>
        /// Delete a Category In Menu by id
        /// </summary>
        //DELETE: api/v1/CateGoryInMenu/{id}
        [HttpDelete("{id}/menus")]
        public async Task<ActionResult> DeleteStoreCategory(string id)
        {
            try
            {
                var result = await repository.Category.DeleteCateInMenuById(id);
                return Ok(result);
            }
            catch (Exception)
            {
                return Ok(new
                {
                    message = "Hiện tại danh mục đang có trong menu !!" +
                                              "Vui lòng xóa danh mục khỏi menu và thử lại "
                });
            }

        }
        /// <summary>
        /// Update Category with pagination
        /// </summary>
        //PUT: api/v1/Category?id
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCategory(string id, CategoryDto category, Boolean imgUpdate)
        {
            try
            {
                var CategoryToUpdate = await repository.Category.UpdateCategoryById(id, category, imgUpdate);
                return Ok(category);
            }
            catch
            {
                return Conflict();
            }
        }
    }
}
