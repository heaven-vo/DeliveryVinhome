using DeliveryVHGP.Core.Data;
using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Core.Interface.IRepositories;
using DeliveryVHGP.Core.Models;
using DeliveryVHGP.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace DeliveryVHGP.WebApi.Repositories
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(DeliveryVHGP_DBContext context) : base(context)
        {
        }
        public async Task<List<AccountModel>> GetAll(int pageIndex, int pageSize)
        {
            var listAccount = await context.Accounts.
                Select(x => new AccountModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Password = x.Password,
                    RoleId = x.RoleId,
                    Status = x.Status,
                }).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return listAccount;
        }

    }
}
