using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DeliveryVHGP_WebApi.Models;
using DeliveryVHGP_WebApi.IRepositories;

namespace DeliveryVHGP_WebApi.Controllers
{
    [Route("api/v1/category")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesRepository _categoriesRepository;

        public CategoriesController(ICategoriesRepository categoriesRepository)
        {
            _categoriesRepository = categoriesRepository;
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
    }
}
