using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Core.Interfaces;
using DeliveryVHGP.Core.Models;

namespace DeliveryVHGP.Core.Interface.IRepositories
{
    public interface IAccountRepository : IRepositoryBase<Account>
    {
        Task<List<AccountModel>> GetAll(int pageIndex, int pageSize);
        Task CreateAcc();
    }
}
