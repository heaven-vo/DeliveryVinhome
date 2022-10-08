using DeliveryVHGP_WebApi.IRepositories;
using DeliveryVHGP_WebApi.Models;
using DeliveryVHGP_WebApi.ViewModels;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.EntityFrameworkCore;

namespace DeliveryVHGP_WebApi.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DeliveryVHGP_DBContext _context;
        public CustomerRepository(DeliveryVHGP_DBContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ViewListCustomer>> GetAll(int pageIndex, int pageSize)
        {
            var listCus = await _context.Customers.
                Select(x => new ViewListCustomer
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    Image = x.Image,
                    Phone = x.Phone,
                    BuildingId = x.BuildingId
                }).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return listCus;
        }
        public async Task<CustomerModels> CreateCustomer(CustomerModels cus)
        {
              var customer = new Customer {
                Id = Guid.NewGuid().ToString(),
                FullName = cus.FullName,
                Image = cus.Image,
                Phone = cus.Phone,
                BuildingId = cus.BuildingId,
            };
            await _context.SaveChangesAsync();
            return cus;

        }


    }
}
