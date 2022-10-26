using DeliveryVHGP.Core.Models.Noti;
using DeliveryVHGP.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryVHGP.WebApi.Controllers
{
    [Route("api/notification")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [Route("send")]
        [HttpPost]
        public async Task<IActionResult> SendNotification(NotificationModel notificationModel)
        {
            //notificationModel.DeviceId = "fUcQMqV7QK6bHsN9hc50Iv:APA91bHpyhsyHZR5X8m7Bm2XghNYliGIzkRe93E0PLPyrRMggPH787gTpIW2THVisgmhT0poRaO2Rhe95eKQLcDVYjvfSxNJViva-RTZHmun8f-0dPMB0sykivdEVjPBUyKSto9PGpzv";
            var result = await _notificationService.SendNotification(notificationModel);
            return Ok(result);
        }
    }
}
