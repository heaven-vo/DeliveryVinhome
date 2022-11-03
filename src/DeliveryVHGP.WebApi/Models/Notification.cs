using System;
using System.Collections.Generic;

namespace DeliveryVHGP.WebApi.Models
{
    public partial class Notification
    {
        public string? Id { get; set; }
        public string? Date { get; set; }
        public string? Time { get; set; }
        public string? NotiContent { get; set; }
        public string? Title { get; set; }
        public string? Image { get; set; }
        public string? UserId { get; set; }

        public virtual Account? User { get; set; }
    }
}
