using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Core.Models;

namespace DeliveryVHGP.Core.Interfaces.IRepositories
{
    public interface ITransactionRepository : IRepositoryBase<Transaction>
    {
        Task<List<TransactionModel>> GetListTransactionByShipperId(string shipperId, int page, int pageSize);
        Task<double> GetBalanceWallet(string accountId);
    }
}
