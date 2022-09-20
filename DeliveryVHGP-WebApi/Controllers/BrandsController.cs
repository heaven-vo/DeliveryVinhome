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
    [Route("api/v2/brands")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandRepository _brandRepository;

        public BrandsController(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        /// <summary>
        /// Get list all Brand with pagination
        /// </summary>
        //GET: api/v1/Brand?pageIndex=1&pageSize=3
        [HttpGet]
        public async Task<ActionResult> GetAll(int pageIndex, int pageSize)
        {
            return Ok(await _brandRepository.GetAll(pageIndex, pageSize));
        }

        /// <summary>
        /// Get a brand by id
        /// </summary>
        //GET: api/v1/brand/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(string id)
        {
            var brand = await _brandRepository.GetById(id);
            if (brand == null)
                return NotFound();
            return Ok(brand);
        }
    }
}