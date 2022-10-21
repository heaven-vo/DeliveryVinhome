using Microsoft.AspNetCore.Mvc;
using DeliveryVHGP.Core.Interfaces;
using DeliveryVHGP.Core.Models;

namespace DeliveryVHGP.WebApi.Controllers
{
    [Route("api/v1/products")]
    [ApiController]
        public class ProductController : ControllerBase
    {
        private readonly IRepositoryWrapper repository;
        public ProductController(IRepositoryWrapper repository)
        {
            this.repository = repository;
        }
        /// <summary>
        /// Get product by id with pagination
        /// </summary>
        //GET: api/v1/productbyId?pageIndex=1&pageSize=3
        [HttpGet("{storeId}/products")]
        public async Task<ActionResult> GetProduct(string storeId,int pageIndex, int pageSize)
        {
            var pro = await repository.Product.GetAll(storeId,pageIndex, pageSize);
            if (pro == null)
                return NotFound();
            return Ok(pro);
        }
        /// <summary>
        /// Get product by id with pagination
        /// </summary>
        //GET: api/v1/productbyId?pageIndex=1&pageSize=3
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDetailsModel>> GetProductDetail(string id)
        {
            var pro = await repository.Product.GetById(id);
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
                var result = await repository.Product.CreatNewProduct(product);
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
        public async Task<ActionResult> UpdateProById(string id, ProductDto product)
        {
            try
            {
                if (id != product.Id)
                {
                    return BadRequest("Product ID mismatch");
                }
                    var productToUpdate = await repository.Product.UpdateProductById(id, product);
                    return Ok(product);
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
        /// Delete product with pagination
        /// </summary>
        //DELETE: api/v1/product?id
        [HttpDelete("{id}")]
        public async Task<Object> DeleteProductById(string id)
        {
            try
            {
                var productDelete = await repository.Product.DeleteProductById(id);

                if (productDelete == null)
                {
                    return NotFound($"Product with Id = {id} not found");
                }
                 await repository.Product.DeleteProductById(id);
                return Ok(new {message = "Deleted products sucessfull !!" });
            }
            catch (Exception)
            {
                return Ok(new { message = "Hiện tại sản phẩm đang có trong menu !!" +
                                              "Vui lòng xóa sản phẩm khỏi menu và thử lại " });
            }
        }
    }
}
