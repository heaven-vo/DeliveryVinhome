using DeliveryVHGP.Core.Interfaces;
using DeliveryVHGP.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryVHGP.WebApi.Controllers
{
    [Route("api/v1/transactions")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IRepositoryWrapper repository;
        public TransactionController(IRepositoryWrapper repository)
        {
            this.repository = repository;
        }
        [HttpGet("byShipper")]
        public async Task<ActionResult<List<TransactionModel>>> GetTransactionsByShipperId(string shipperId, int page, int pageSize)
        {
            try
            {
                var listTransactions = await repository.Transaction.GetListTransactionByShipperId(shipperId, page, pageSize);
                return Ok(new { StatusCode = "Successful", data = listTransactions });
            }
            catch (Exception e)
            {
                return Ok(new
                {
                    StatusCode = "Fail",
                    message = e.Message
                });
            }
        }
        [HttpPut]
        public async Task<ActionResult> MinusWalletBalanceByAdmin(string AccountId, int walletType, double amonut)
        {
            try
            {
                await repository.Transaction.MinusWalletBalance(AccountId, walletType, amonut);
                return Ok(new { StatusCode = "Successful" });
            }
            catch (Exception e)
            {
                return Ok(new
                {
                    StatusCode = "Fail",
                    message = e.Message
                });
            }
        }
    }
}
