using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Core.Interfaces;
using DeliveryVHGP.Core.Models;

namespace DeliveryVHGP.Core.Interface.IRepositories
{
    public interface ICustomerRepository : IRepositoryBase<Customer>
    {
        Task<CustomerModels> CreateCustomer(CustomerModels cus);
        Task<IEnumerable<ViewListCustomer>> GetAll(int pageIndex, int pageSize);
    }
}
