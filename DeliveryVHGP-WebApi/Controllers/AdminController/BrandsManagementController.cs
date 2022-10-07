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
    [Route("api/v2/brand-management/brands")]
    [ApiController]
    public class BrandsManagementController : ControllerBase
    {
        private readonly IBrandRepository _brandRepository;

        public BrandsManagementController(IBrandRepository brandRepository)
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
        /// <summary>
        /// Create a brand
        /// </summary>
        //POST: api/v1/brand
        [HttpPost]
        public async Task<ActionResult> CreateBrand(BrandModels brand)
        {
            try
            {
                var result = await _brandRepository.CreateBrand(brand);
                return Ok(result);
            }
            catch
            {
                return Conflict();
            }
        }
        /// <summary>
        /// Delete a brand by id
        /// </summary>
        //DELETE: api/v1/brand/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBrand(string id)
        {
            try
            {
                var result = await _brandRepository.DeleteById(id);
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
        public async Task<ActionResult> UpdateBrand(string id, BrandModels brand)
        {
            try
            {
                if (id != brand.Id)
                {
                    return BadRequest("Brand ID mismatch");
                }
                var BrandToUpdate = await _brandRepository.UpdateBrandById(id, brand);
                return Ok(brand);
            }
            catch
            {
                return Conflict();
            }
        }
        ///// <summary>
        ///// Create Upload image to Firebase
        ///// </summary>
        /////POST: api/v1/brand
        [HttpPost("UploadFile")]
        public async Task<ActionResult> PostFireBase(IFormFile file)
        {
            var fileUpload = file;
            try
            {
                if (fileUpload.Length > 0)
                {
                    var upBrand = await _brandRepository.PostFireBase(file);
                    return Ok(new { StatusCode = 200, message = "Upload file succesful!" });
                }
                return BadRequest("Upload  fail");
            }
            catch (Exception e)
            {
                return StatusCode(409, new { StatusCode = 409, message = e.Message });
            }
        }
    }
}