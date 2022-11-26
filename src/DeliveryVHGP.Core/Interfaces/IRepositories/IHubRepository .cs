using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Core.Interfaces;
using DeliveryVHGP.Core.Models;

namespace DeliveryVHGP.Core.Interface.IRepositories
{
    public interface IHubRepository : IRepositoryBase<Hub>
    {
        Task<IEnumerable<HubModels>> GetlistHub(int pageIndex, int pageSize, FilterRequestInHub request);
        Task<HubModels> GetById(string id);
        Task<HubDto> CreateHub(HubDto hub);
        Task<Object> DeleteById(string hubId);
        Task<Object> UpdateHubById(string hubId, HubModels hub);
    }
}
