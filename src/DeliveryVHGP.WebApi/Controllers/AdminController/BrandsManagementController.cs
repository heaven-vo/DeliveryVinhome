using Microsoft.AspNetCore.Mvc;
using DeliveryVHGP.Core.Interfaces;
using DeliveryVHGP.Core.Models;

namespace DeliveryVHGP.WebApi.Controllers
{
    [Route("api/v2/brand-management/brands")]
    [ApiController]
    public class BrandsManagementController : ControllerBase
    {
        private readonly IRepositoryWrapper repository;
        public BrandsManagementController(IRepositoryWrapper repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Get list all Brand with pagination
        /// </summary>
        //GET: api/v1/Brand?pageIndex=1&pageSize=3
        [HttpGet]
        public async Task<ActionResult> GetAll(int pageIndex, int pageSize)
        {
            return Ok(await repository.Brand.GetAll(pageIndex, pageSize));
        }

        /// <summary>
        /// Get a brand by id
        /// </summary>
        //GET: api/v1/brand/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(string id)
        {
            var brand = await repository.Brand.GetById(id);
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
                var result = await repository.Brand.CreateBrand(brand);
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
                var result = await repository.Brand.DeleteById(id);
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
                var BrandToUpdate = await repository.Brand.UpdateBrandById(id, brand);
                return Ok(brand);
            }
            catch
            {
                return Conflict();
            }
        }
    }
}