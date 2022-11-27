using DeliveryVHGP.Core.Data;
using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Core.Interfaces.IRepositories;
using DeliveryVHGP.Core.Models;
using DeliveryVHGP.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace DeliveryVHGP.Infrastructure.Repositories
{
    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        public TransactionRepository(DeliveryVHGP_DBContext context) : base(context)
        {
        }
        public async Task<List<TransactionModel>> GetListTransactionByShipperId(string shipperId, int page, int pageSize)
        {
            var walletId = await context.Wallets.Where(x => x.AccountId == shipperId).Select(x => x.Id).FirstOrDefaultAsync();
            var listTrans = await context.Transactions.Where(x => x.WalletId == walletId).OrderByDescending(x => x.CreateAt)
                .Select(x => new TransactionModel
                {
                    Amount = x.Amount,
                    TransactionType = x.Type,
                    Date = x.CreateAt,
                    TransactionAction = x.Action,
                    Status = x.Status
                }).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return listTrans;
        }
    }
}
