using DeliveryVHGP.Core.Data;
using DeliveryVHGP.Core.Entities;
using DeliveryVHGP.Core.Enums;
using DeliveryVHGP.Core.Interface.IRepositories;
using DeliveryVHGP.Core.Models;
using DeliveryVHGP.Infrastructure.Repositories.Common;
using DeliveryVHGP.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace DeliveryVHGP.WebApi.Repositories
{
    public class ShipperRepository : RepositoryBase<Shipper>, IShipperRepository
    {
        private readonly IFileService _fileService;
        private readonly ITimeStageService _timeStageService;
        public ShipperRepository(ITimeStageService timeStageService, IFileService fileService, DeliveryVHGP_DBContext context) : base(context)
        {
            _fileService = fileService;
            _timeStageService = timeStageService;
        }
        public async Task<IEnumerable<ShipperModel>> GetListShipper(int pageIndex, int pageSize, FilterRequestInShipper request)
        {
            var listShipper = await (from ship in context.Shippers
                                     join acc in context.Accounts on ship.Id equals acc.Id
                                     where ship.FullName.Contains(request.SearchByName)
                                     select new ShipperModel()
                                     {
                                         Id = ship.Id,
                                         FullName = ship.FullName,
                                         Phone = ship.Phone,
                                         Image = ship.Image,
                                         Email = ship.Email,
                                         VehicleType = ship.VehicleType,
                                         DeliveryTeam = ship.DeliveryTeam,
                                         LicensePlates = ship.LicensePlates,
                                         Password = acc.Password,
                                         Colour = ship.Colour,
                                         Status = ship.Status,
                                         CreateAt = ship.CreateAt,
                                         UpdateAt = ship.UpdateAt,

                                     }).OrderByDescending(t => t.CreateAt).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return listShipper;
        }
        public async Task<Object> GetShipperById(string shipId)
        {
            var shipper = await (from ship in context.Shippers.Where(x => x.Id == shipId)

                                 join acc in context.Accounts on ship.Id equals acc.Id
                                 select new ShipperModel()
                                 {
                                     Id = ship.Id,
                                     FullName = ship.FullName,
                                     Phone = ship.Phone,
                                     Email = ship.Email,
                                     Image = ship.Image,
                                     VehicleType = ship.VehicleType,
                                     DeliveryTeam = ship.DeliveryTeam,
                                     LicensePlates = ship.LicensePlates,
                                     Colour = ship.Colour,
                                     Status = ship.Status,
                                     Password = acc.Password,
                                     CreateAt = ship.CreateAt,
                                     UpdateAt = ship.UpdateAt

                                 }).FirstOrDefaultAsync();
            return shipper;
        }
        public async Task<IEnumerable<ShipperModel>> GetListShipperByName(string shipName, int pageIndex, int pageSize)
        {
            var listShipper = await (from ship in context.Shippers.Where(ship => ship.FullName.Contains(shipName))

                                     join acc in context.Accounts on ship.Id equals acc.Id
                                     select new ShipperModel()
                                     {
                                         Id = ship.Id,
                                         FullName = ship.FullName,
                                         Phone = ship.Phone,
                                         Image = ship.Image,
                                         Email = ship.Email,
                                         VehicleType = ship.VehicleType,
                                         DeliveryTeam = ship.DeliveryTeam,
                                         LicensePlates = ship.LicensePlates,
                                         Password = acc.Password,
                                         Colour = ship.Colour,
                                         Status = ship.Status,
                                         CreateAt = ship.CreateAt,
                                         UpdateAt = ship.UpdateAt,

                                     }).OrderByDescending(t => t.CreateAt).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return listShipper;
        }
        public async Task<ShipperDto> CreateShipper(ShipperDto ship)
        {
            string fileImg = "ImagesShipper";
            string time = await _timeStageService.GetTime();
            context.Shippers.Add(
                new Shipper
                {
                    Id = ship.Id,
                    FullName = ship.FullName,
                    Phone = ship.Phone,
                    Email = ship.Email,
                    VehicleType = ship.VehicleType,
                    DeliveryTeam = ship.DeliveryTeam,
                    Image = await _fileService.UploadFile(fileImg, ship.Image),
                    LicensePlates = ship.LicensePlates,
                    Status = true,
                    Colour = ship.Colour,
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
            context.Wallets.Add(
               new Wallet
               {
                   Id = Guid.NewGuid().ToString(),
                   AccountId = ship.Id,
                   Type = (int)WalletTypeEnum.Refund,
                   Amount = 0,
                   Active = true,

               });
            context.Wallets.Add(
              new Wallet
              {
                  Id = Guid.NewGuid().ToString(),
                  AccountId = ship.Id,
                  Type = (int)WalletTypeEnum.Debit,
                  Amount = 0,
                  Active = true,

              });
            await context.SaveChangesAsync();
            return ship;
        }
        public async Task<ShipperDto> UpdateShipper(string shipId, ShipperDto ship, Boolean imgUpdate)
        {
            string fileImg = "ImagesShipper";
            string time = await _timeStageService.GetTime();
            var result = await context.Shippers.FindAsync(shipId);
            var account = context.Accounts.FirstOrDefault(x => x.Id == shipId);

            result.Id = shipId;
            result.FullName = ship.FullName;
            result.Phone = ship.Phone;
            result.Email = ship.Email;
            result.VehicleType = ship.VehicleType;
            result.DeliveryTeam = ship.DeliveryTeam;
            result.Colour = ship.Colour;
            result.LicensePlates = ship.LicensePlates;
            if (imgUpdate == true)
            {
                result.Image = await _fileService.UploadFile(fileImg, ship.Image);
            }
            account.Password = ship.Password;
            account.Name = ship.FullName;
            result.UpdateAt = time;
            try
            {
                await context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
            return ship;
        }
        public async Task<StatusShipDto> UpdateStatusShipper(string ShipId, StatusShipDto shipper)
        {
            var result = await context.Shippers.FindAsync(ShipId);
            var segmentDeli = context.SegmentDeliveryRoutes.FirstOrDefault(sd => sd.ShipperId == ShipId);
            if (segmentDeli != null)
            {
                var edge = context.RouteEdges.FirstOrDefault(e => e.RouteId == segmentDeli.Id);
                if (edge != null)
                {
                    var oAction = context.OrderActions.FirstOrDefault(oa => oa.RouteEdgeId == edge.Id);
                    if(oAction != null)
                    {
                        var order = context.Orders.FirstOrDefault(x => x.Id == oAction.OrderId);
                        if (order != null)
                        {
                            //var OrderStatus = context.OrderStatuses.FirstOrDefault(os => os.Id == status.Status);
                            if (order.Status == (int)OrderStatusEnum.Fail || order.Status == (int)OrderStatusEnum.Completed || order.Status == (int)FailStatus.OutTime
                                                    || order.Status == (int)FailStatus.StoreFail || order.Status == (int)FailStatus.ShipperFail || order.Status == (int)FailStatus.CustomerFail)
                            {
                                result.Status = shipper.Status;
                            }
                            if (order.Status == (int)OrderStatusEnum.New || order.Status == (int)OrderStatusEnum.Received || order.Status == (int)OrderStatusEnum.Assigning
                                || order.Status == (int)OrderStatusEnum.Accepted || order.Status == (int)OrderStatusEnum.InProcess || order.Status == (int)InProcessStatus.HubDelivery
                                || order.Status == (int)InProcessStatus.AtHub || order.Status == (int)InProcessStatus.CustomerDelivery)
                                throw new Exception("Hiện tại đang có đơn hàng chưa hoàn thành!!" +
                                                             "Vui lòng kiểm tra lại đơn hàng và thử lại");
                        }
                    }
                }
            }
            if (segmentDeli == null)
            {
                result.Status = shipper.Status;
            }
                try
            {
                context.Entry(result).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
            return shipper;
        }
        public async Task<Object> DeleteShipper(string shipperId)
        {
            var deShip = await context.Shippers.FindAsync(shipperId);
            var segmentDeli = context.SegmentDeliveryRoutes.FirstOrDefault(sd => sd.ShipperId == shipperId);
            //context.Shippers.Remove(deShip);
            if (segmentDeli == null)
            {
                context.Shippers.Remove(deShip);
            }
            if (segmentDeli != null)
            {
                var edge = context.RouteEdges.FirstOrDefault(e => e.RouteId == segmentDeli.Id);
                if (edge != null)
                {
                    var oAction = context.OrderActions.FirstOrDefault(oa => oa.RouteEdgeId == edge.Id);
                    if (oAction != null)
                    {
                        var order = context.Orders.FirstOrDefault(x => x.Id == oAction.OrderId);
                        var tran = await context.Transactions.Where(t => t.OrderId == order.Id).ToListAsync();
                        if (order != null)
                        {
                            //var OrderStatus = context.OrderStatuses.FirstOrDefault(os => os.Id == status.Status);
                            if (order.Status == (int)OrderStatusEnum.Fail || order.Status == (int)OrderStatusEnum.Completed || order.Status == (int)FailStatus.OutTime
                                                    || order.Status == (int)FailStatus.StoreFail || order.Status == (int)FailStatus.ShipperFail || order.Status == (int)FailStatus.CustomerFail)
                            {
                                context.Shippers.Remove(deShip);
                                context.Transactions.RemoveRange(tran);
                            }
                            if (order.Status == (int)OrderStatusEnum.New || order.Status == (int)OrderStatusEnum.Received || order.Status == (int)OrderStatusEnum.Assigning
                                || order.Status == (int)OrderStatusEnum.Accepted || order.Status == (int)OrderStatusEnum.InProcess || order.Status == (int)InProcessStatus.HubDelivery
                                || order.Status == (int)InProcessStatus.AtHub || order.Status == (int)InProcessStatus.CustomerDelivery)
                                throw new Exception("Hiện tại đang có đơn hàng chưa hoàn thành!!" +
                                                             "Vui lòng kiểm tra lại đơn hàng và thử lại");
                        }
                    }
                }
            }
            //var shipperHis = context.ShipperHistories.FirstOrDefault(s => s.ShipperId == s.ShipperId);
            //var order =  context.Orders.FirstOrDefault(o => o.Id == shipperHis.Id);
            var account = await context.Accounts.FindAsync(shipperId);
            context.Accounts.Remove(account);
            var wallet = await context.Wallets.Where(w => w.AccountId == shipperId).ToListAsync();
            context.Wallets.RemoveRange(wallet);
            //var tran = await context.Transactions.Where(t => t.OrderId == order.Id).ToListAsync();
            //context.Transactions.RemoveRange(tran);
            await context.SaveChangesAsync();

            return deShip;
        }
    }
}
