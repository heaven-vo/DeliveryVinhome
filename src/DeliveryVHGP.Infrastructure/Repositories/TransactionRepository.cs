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
        public async Task MinusWalletBalance(string AccountId, int walletType, double amonut)
        {
            var wallet = await context.Wallets.Where(x => x.AccountId == AccountId && x.Type == walletType).FirstOrDefaultAsync();
            if (wallet == null)
            {
                throw new Exception("Tài khoản không hợp lệ");
            }
            if (amonut > wallet.Amount && walletType == (int)WalletTypeEnum.Refund)
            {
                throw new Exception("Số tiền trong tài khoản không đủ để thực hiện giao dịch");
            }
            if (amonut > wallet.Amount && walletType == (int)WalletTypeEnum.Debit)
            {
                throw new Exception("Số tiền lớn hơn mức cần hoàn lại");
            }
            if (amonut > wallet.Amount && walletType == (int)WalletTypeEnum.Commission)
            {
                throw new Exception("Số tiền lớn hơn mức hoa hồng cần trả");
            }
            wallet.Amount -= amonut;
            Transaction transaction = new Transaction()
            {
                Id = Guid.NewGuid().ToString(),
                Action = (int)TransactionActionEnum.minus,
                Amount = amonut,
                CreateAt = DateTime.UtcNow.AddHours(7),
                WalletId = wallet.Id,
                Type = (int)TransactionTypeEnum.recharge,
                Status = (int)StatusEnum.success
            };
            if (walletType == (int)WalletTypeEnum.Debit || walletType == (int)WalletTypeEnum.Commission)
            {
                transaction.Type = (int)TransactionTypeEnum.recharge;
            }
            if (walletType == (int)WalletTypeEnum.Refund)
            {
                transaction.Type = (int)TransactionTypeEnum.withdraw;
            }
            await context.Transactions.AddAsync(transaction);
            await context.SaveChangesAsync();
        }
    }
}
