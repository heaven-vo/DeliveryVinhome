using DeliveryVHGP_WebApi.ViewModels;

namespace DeliveryVHGP_WebApi.IRepositories
{
    public interface ICustomerRepository
    {
        Task<CustomerModels> CreateCustomer(CustomerModels cus);
        Task<IEnumerable<ViewListCustomer>> GetAll(int pageIndex, int pageSize);
    }
}
