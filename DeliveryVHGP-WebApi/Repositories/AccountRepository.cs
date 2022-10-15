using DeliveryVHGP_WebApi.IRepositories;
using DeliveryVHGP_WebApi.Models;
using DeliveryVHGP_WebApi.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DeliveryVHGP_WebApi.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DeliveryVHGP_DBContext _context;
        public AccountRepository(DeliveryVHGP_DBContext context)
        {
            _context = context;
        }
        public async Task<List<AccountModel>> GetAll(int pageIndex, int pageSize)
        {
            var listAccount = await _context.Accounts.
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
