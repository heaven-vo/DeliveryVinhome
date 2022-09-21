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
    [Route("api/v1/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly IMenuRepository _menuRepository;

        public CategoriesController(ICategoriesRepository categoriesRepository, IMenuRepository menuRepository)
        {
            _categoriesRepository = categoriesRepository;
            _menuRepository = menuRepository;
        }
        /// <summary>
        /// Get list all category with pagination
        /// </summary>
        //GET: api/v1/category?pageIndex=1&pageSize=3
        [HttpGet]
        public async Task<ActionResult> GetAll(int pageIndex, int pageSize)
        {
            return Ok(await _categoriesRepository.GetAll(pageIndex, pageSize));
        }
        /// <summary>
        /// Get list Category in a menu
        /// </summary>
        [HttpGet("menus")]
        public async Task<ActionResult<List<ProductViewInList>>> GetAllProductInMenu(string menuId, int page, int pageSize)
        {
            return Ok(await _categoriesRepository.GetListCategoryByMenuId(menuId, page, pageSize));
        }
        /// <summary>
        /// Create a category
        /// </summary>
        //POST: api/v1/category
        [HttpPost]
        public async Task<ActionResult> CreateCategory(CategoryModel category)
        {
            try
            {
                var result = await _categoriesRepository.CreateCategory(category);
                return Ok(result);
            }
            catch
            {
                return Conflict();
            }
        }
        /// <summary>
        /// Create new Category in Menu 
        /// </summary>
        [HttpPost("menus")]
        public async Task<ActionResult> CreateCategoryInMenu(CategoryInMenuModel newCate)
        {
            try
            {
                var result = await _categoriesRepository.CreateCategoryByMenuId(newCate);
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
                var result = await _categoriesRepository.DeleteCateInMenuById(id);
                return Ok(result);
            }
            catch
            {
                return Conflict();
            }

        }
        /// <summary>
        /// Update Category with pagination
        /// </summary>
        //PUT: api/v1/Category?id
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCategory(string id, CategoryModel category)
        {
            try
            {
                if (id != category.Id)
                {
                    return BadRequest("Category ID mismatch");
                }
                var CategoryToUpdate = await _categoriesRepository.UpdateCategoryById(id, category);
                return Ok(category);
            }
            catch
            {
                return Conflict();
            }
        }
    }
}
