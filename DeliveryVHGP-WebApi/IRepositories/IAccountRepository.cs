using DeliveryVHGP_WebApi.ViewModels;

namespace DeliveryVHGP_WebApi.IRepositories
{
    public interface IAccountRepository
    {
        Task<List<AccountModel>> GetAll(int pageIndex, int pageSize);
    }
}
