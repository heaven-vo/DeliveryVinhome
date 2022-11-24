using Google.Cloud.Firestore;

namespace DeliveryVHGP.Core.Models
{
    [FirestoreData]
    public class RouteUpdateModel
    {
        [FirestoreProperty]
        public string? ShipperId { get; set; }
        [FirestoreProperty]
        public int? Status { get; set; }
    }
}
