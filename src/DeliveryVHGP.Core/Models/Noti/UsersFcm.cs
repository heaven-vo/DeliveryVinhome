using Google.Cloud.Firestore;

namespace DeliveryVHGP.Core.Models.Noti
{
    [FirestoreData]
    public class UsersFcm
    {
        [FirestoreProperty]
        public string email { get; set; }
        [FirestoreProperty]
        public List<string> fcmToken { get; set; }
    }
}
