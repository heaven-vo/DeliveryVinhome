using DeliveryVHGP.Core.Interface.IRepositories;
using DeliveryVHGP.Core.Data;
using DeliveryVHGP.Core.Models;
using Microsoft.EntityFrameworkCore;
using DeliveryVHGP.Infrastructure.Services;
using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Infrastructure.Repositories.Common;

namespace DeliveryVHGP.WebApi.Repositories
{
    public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
    {
        private readonly IFileService _fileService;

        public CustomerRepository(IFileService fileService, DeliveryVHGP_DBContext context): base(context)
        {
            _fileService = fileService;
        }
        public async Task<IEnumerable<ViewListCustomer>> GetAll(int pageIndex, int pageSize)
        {
            var listCus = await context.Customers.
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
            string fileImg = "ImagesCustomers";
            context.Customers.Add(
                new Customer {
                Id = Guid.NewGuid().ToString(),
                FullName = cus.FullName,
                Image = await _fileService.UploadFile(fileImg, cus.Image),
                Phone = cus.Phone,
                BuildingId = cus.BuildingId
            });
            await context.SaveChangesAsync();
            return cus;

        }


    }
}
