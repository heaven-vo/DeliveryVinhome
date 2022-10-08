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
    [Route("api/v1/customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        /// <summary>
        /// Get list all Customer with pagination
        /// </summary>
        //GET: api/v1/Customer?pageIndex=1&pageSize=3
        [HttpGet]
        public async Task<ActionResult> GetAll(int pageIndex, int pageSize)
        {
            return Ok(await _customerRepository.GetAll(pageIndex, pageSize));
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
                var result = await _customerRepository.CreateCustomer(cus);
                return Ok(result);
            }
            catch
            {
                return Conflict();
            }
        }

    }
}