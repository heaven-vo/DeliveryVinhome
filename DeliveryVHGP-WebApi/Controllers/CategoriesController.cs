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
        /// Get a category by id
        /// </summary>
        //GET: api/v1/category/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(string id)
        {
            var category = await _categoriesRepository.GetById(id);
            if (category == null)
                return NotFound();
            return Ok(category);
        }

        /// <summary>
        /// Get list menu in category
        /// </summary>
        //GET: api/v1/category/{id}/menus
        [HttpGet("{id}/menus")]
        public async Task<ActionResult<List<MenuView>>> GetListMenuInCategory(string menuId)
        {
            return Ok(await _menuRepository.GetListMenuByCategoryId(menuId));
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
        /// Update Brand with pagination
        /// </summary>
        //PUT: api/v1/Brand?id
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
