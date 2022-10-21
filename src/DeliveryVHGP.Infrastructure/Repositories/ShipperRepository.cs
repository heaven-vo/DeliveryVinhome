using DeliveryVHGP.Core.Interface.IRepositories;
using DeliveryVHGP.Core.Data;
using DeliveryVHGP.Core.Models;
using Microsoft.EntityFrameworkCore;
using DeliveryVHGP.Infrastructure.Services;
using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Infrastructure.Repositories.Common;

namespace DeliveryVHGP.WebApi.Repositories
{
    public class ShipperRepository : RepositoryBase<Shipper>, IShipperRepository
    {
        private readonly IFileService _fileService;
        private readonly ITimeStageService _timeStageService;
        public ShipperRepository(ITimeStageService timeStageService, IFileService fileService, DeliveryVHGP_DBContext context): base(context)
        {
            _fileService = fileService;
            _timeStageService = timeStageService;
        }
        public async Task<IEnumerable<ShipperModel>> GetListShipper(int pageIndex, int pageSize)
        {
            var listShipper = await (from ship in context.Shippers
                                   
                                   select new ShipperModel()
                                   {
                                       Id = ship.Id,
                                       FullName = ship.FullName,
                                       Phone = ship.Phone,
                                       Image = ship.Image,
                                       Email = ship.Email,
                                       VehicleType = ship.VehicleType,
                                       DeliveryTeam = ship.DeliveryTeam,
                                       Status = ship.Status,
                                       CreateAt = ship.CreateAt,
                                       UpdateAt = ship.UpdateAt,

                                   }).OrderByDescending(t => t.CreateAt).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return listShipper;
        }
        public async Task<Object> GetShipperById(string shipId)
        {
            var shipper = await context.Shippers.Where(x => x.Id == shipId)
                                     .Select(ship => new ShipperModel
                                     {
                                         Id = ship.Id,
                                         FullName = ship.FullName,
                                         Phone = ship.Phone,
                                         Email = ship.Email,
                                         Image = ship.Image,
                                         VehicleType = ship.VehicleType,
                                         DeliveryTeam = ship.DeliveryTeam,
                                         Status = ship.Status,
                                         CreateAt = ship.CreateAt,
                                         UpdateAt = ship.UpdateAt

                                     }).FirstOrDefaultAsync();
            return shipper;
        }
        public async Task<ShipperDto> CreateShipper(ShipperDto ship)
        {
            string fileImg = "ImagesShipper";
            string time = await _timeStageService.GetTime();
            context.Shippers.Add(
                new Shipper
                {
                    Id = ship.Id,
                    FullName=ship.FullName,
                    Phone= ship.Phone,
                    Email=ship.Email,
                    VehicleType=ship.VehicleType,
                    DeliveryTeam=ship.DeliveryTeam,
                    Image = await _fileService.UploadFile(fileImg, ship.Image),
                    CreateAt = time
                });
            context.Accounts.Add(
               new Account
               {
                   Id = ship.Id,
                   Name = ship.FullName,
                   Password = ship.Password,
                   RoleId = "3",
                   Status = "true"
               });
            await context.SaveChangesAsync();
            return ship;
        }
    }
}
