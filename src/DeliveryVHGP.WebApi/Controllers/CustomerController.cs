using Microsoft.AspNetCore.Mvc;
using DeliveryVHGP.Core.Interfaces;
using DeliveryVHGP.Core.Models;

namespace DeliveryVHGP.WebApi.Controllers
{
    [Route("api/v1/customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IRepositoryWrapper repository;
        public CustomerController(IRepositoryWrapper repository)
        {
            this.repository = repository;
        }
        /// <summary>
        /// Get list all Customer with pagination
        /// </summary>
        //GET: api/v1/Customer?pageIndex=1&pageSize=3
        [HttpGet]
        public async Task<ActionResult> GetAll(int pageIndex, int pageSize)
        {
            return Ok(await repository.Customer.GetAll(pageIndex, pageSize));
        }
        /// <summary>
        /// Create a customer (customer web)
        /// </summary>
        //POST: api/v1/Customer
        [HttpPost]
        public async Task<ActionResult> CreateCustomer(CustomerModels cus)
        {
            try
            {
                var result = await repository.Customer.CreateCustomer(cus);
                return Ok(result);
            }
            catch
            {
                return Conflict();
            }
        }

    }
}