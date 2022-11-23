using DeliveryVHGP.Core.Models;

namespace DeliveryVHGP.Infrastructure.Services
{
    public interface IFirestoreService
    {
        Task AddEmployee(RouteModel route);
        Task UpdateEmployee(RouteModel route);
        Task<RouteModel> GetEmployeeData(string id);
        Task DeleteAllEmployees();
    }
}
