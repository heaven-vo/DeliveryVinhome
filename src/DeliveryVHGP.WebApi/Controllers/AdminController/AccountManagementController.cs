using Microsoft.AspNetCore.Mvc;
using DeliveryVHGP.Core.Interfaces;

namespace DeliveryVHGP.WebApi.Controllers
{
    [Route("api/v1/account-management/accounts")]
    [ApiController]
    public class AccountManagementController : ControllerBase
    {
        private readonly IRepositoryWrapper repository;
        public AccountManagementController(IRepositoryWrapper repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Get list all Account with pagination
        /// </summary>
        //GET: api/v1/Account?pageIndex=1&pageSize=3
        [HttpGet]
        public async Task<ActionResult> GetAll(int pageIndex, int pageSize)
        {
            return Ok(await repository.Account.GetAll(pageIndex, pageSize));
        }
        /// <summary>
        /// Check Account with pagination
        /// </summary>
        //GET: api/v1/Account?pageIndex=1&pageSize=3
        [HttpGet("check-Account")]
        public async Task<ActionResult> CheckAccount(string id)
        {
            try
            {
                return Ok(await repository.Account.CheckAccount(id));
            }
            catch 
            {
                return NotFound();
            }
        }
    }
}