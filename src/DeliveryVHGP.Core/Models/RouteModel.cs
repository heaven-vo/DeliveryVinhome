using Google.Cloud.Firestore;

namespace DeliveryVHGP.Core.Models
{
    [FirestoreData]
    public class RouteModel
    {
        [FirestoreProperty]
        public string RouteId { get; set; }
        [FirestoreProperty]
        public string FirstEdge { get; set; }
        [FirestoreProperty]
        public string LastEdge { get; set; }
        [FirestoreProperty]
        public int EdgeNum { get; set; }
        [FirestoreProperty]
        public int OrderNum { get; set; }
        [FirestoreProperty]
        public double? TotalAdvance { get; set; }
        [FirestoreProperty]
        public double? TotalCod { get; set; }
        [FirestoreProperty]
        public string? ShipperId { get; set; }
        [FirestoreProperty]
        public int? Status { get; set; }
        [FirestoreProperty]
        public int? Type { get; set; }

    }
}
