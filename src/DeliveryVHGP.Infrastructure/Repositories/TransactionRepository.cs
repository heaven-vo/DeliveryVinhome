using DeliveryVHGP.Core.Data;
using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Core.Enums;
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
        public async Task<WalletsShipperModel> GetBalanceWallet(string accountId)
        {
            var refundBalance = await context.Wallets.Where(x => x.AccountId == accountId && x.Type == (int)WalletTypeEnum.Refund && x.Active == true).Select(x => x.Amount).FirstOrDefaultAsync();
            var debitBalance = await context.Wallets.Where(x => x.AccountId == accountId && x.Type == (int)WalletTypeEnum.Debit && x.Active == true).Select(x => x.Amount).FirstOrDefaultAsync();
            if (refundBalance == null || debitBalance == null)
            {
                throw new Exception("Account's wallet not avaliable");
            }
            WalletsShipperModel wallet = new WalletsShipperModel { refundBalance = refundBalance, debitBalance = debitBalance };
            ;
            return wallet;
        }
        public async Task<List<TransactionModel>> GetListTransactionByShipperId(string shipperId, int page, int pageSize)
        {
            var ListWalletId = await context.Wallets.Where(x => x.AccountId == shipperId).Select(x => x.Id).ToListAsync();
            if (!ListWalletId.Any())
            {
                throw new Exception("Shipper's wallet not found");
            }
            var listTrans = await context.Transactions.Where(x => ListWalletId.Contains(x.WalletId)).OrderByDescending(x => x.CreateAt)
                .Select(x => new TransactionModel
                {
                    Amount = x.Amount,
                    TransactionType = x.Type,
                    Date = x.CreateAt,
                    TransactionAction = x.Action,
                    OrderId = x.OrderId,
                    Status = x.Status
                }).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            foreach (var transaction in listTrans)
            {
                if (transaction.TransactionType == (int)TransactionTypeEnum.refund || transaction.TransactionType == (int)TransactionTypeEnum.withdraw)
                {
                    transaction.walletType = (int)WalletTypeEnum.Refund;
                }
                if (transaction.TransactionType == (int)TransactionTypeEnum.cod || transaction.TransactionType == (int)TransactionTypeEnum.shippingcost
                    || transaction.TransactionType == (int)TransactionTypeEnum.recharge)
                {
                    transaction.walletType = (int)WalletTypeEnum.Debit;
                }

            }
            return listTrans;
        }
    }
}
