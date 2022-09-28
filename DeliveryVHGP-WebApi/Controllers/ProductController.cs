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
    [Route("api/v1/products")]
    [ApiController]
        public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        /// <summary>
        /// Get product by id with pagination
        /// </summary>
        //GET: api/v1/productbyId?pageIndex=1&pageSize=3
        [HttpGet("{storeId}/products")]
        public async Task<ActionResult> GetProduct(string storeId,int pageIndex, int pageSize)
        {
            var pro = await _productRepository.GetAll(storeId,pageIndex, pageSize);
            if (pro == null)
                return NotFound();
            return Ok(pro);
        }
        /// <summary>
        /// Get product by id with pagination
        /// </summary>
        //GET: api/v1/productbyId?pageIndex=1&pageSize=3
        [HttpGet("{id}")]
        public async Task<ActionResult> GetProductDetail(string id)
        {
            var pro = await _productRepository.GetById(id);
            if (pro == null)
                return NotFound();
            return Ok(pro);
        }
        /// <summary>
        /// Create a product
        /// </summary>
        //POST: api/v1/product
        [HttpPost]
        public async Task<ActionResult> CreateProduct(ProductModel product)
        {
            try
            {
                var result = await _productRepository.CreatNewProduct(product);
                return Ok(result);
            }
            catch
            {
                return Conflict();
            }
        }
        /// <summary>
        /// Update product Detail with pagination
        /// </summary>
        //PUT: api/v1/productDetail?id
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProById(string id, ProductDetailsModel product)
        {
            try
            {
                if (id != product.Id)
                {
                    return BadRequest("Product ID mismatch");
                }
                    var productToUpdate = await _productRepository.UpdateProductDetailById(id, product);
                    return Ok(product);
            }
            catch
            {
                return Conflict();
            }
        }
        /// <summary>
        /// Delete product with pagination
        /// </summary>
        //DELETE: api/v1/product?id
        [HttpDelete("{id}")]
        public async Task<Object> DeleteProductById(string id)
        {
            try
            {
                var productDelete = await _productRepository.DeleteProductById(id);

                if (productDelete == null)
                {
                    return NotFound($"Product with Id = {id} not found");
                }

                return await _productRepository.DeleteProductById(id);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }
        }


    }
}
