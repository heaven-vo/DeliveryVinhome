using DeliveryVHGP.Core.Models;
using Google.Cloud.Firestore;
using Newtonsoft.Json;

namespace DeliveryVHGP.Infrastructure.Services
{
    public class FirestoreService : IFirestoreService
    {
        string projectId;
        FirestoreDb fireStoreDb;
        public FirestoreService()
        {
            //string filepath = "C:\\Users\\DUONGAS\\Downloads\\deliveryfood-9c436-e35107df256b.json";
            string filepath = "deliveryfood-9c436-e35107df256b.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filepath);
            projectId = "deliveryfood-9c436";
            fireStoreDb = FirestoreDb.Create(projectId);
        }
        public async Task AddRoute(RouteModel route)
        {

            DocumentReference document = fireStoreDb.Collection("employees").Document(route.RouteId);
            await document.SetAsync(route);


        }
        public async Task UpdateRoute(string routeId, RouteUpdateModel route)
        {
            try
            {
                DocumentReference empRef = fireStoreDb.Collection("employees").Document(routeId);
                await empRef.SetAsync(route, SetOptions.MergeAll);
            }
            catch
            {
                throw;
            }
        }
        public async Task<RouteModel> GetRouteData(string id)
        {
            try
            {
                DocumentReference docRef = fireStoreDb.Collection("employees").Document(id);
                DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

                if (snapshot.Exists)
                {
                    RouteModel emp = snapshot.ConvertTo<RouteModel>();
                    emp.RouteId = snapshot.Id;
                    return emp;
                }
                else
                {
                    return new RouteModel();
                }
            }
            catch
            {
                throw;
            }
        }
        public async Task DeleteAllRoutes()
        {
            try
            {
                Query employeeQuery = fireStoreDb.Collection("employees");
                QuerySnapshot employeeQuerySnapshot = await employeeQuery.GetSnapshotAsync();

                foreach (DocumentSnapshot documentSnapshot in employeeQuerySnapshot.Documents)
                {
                    if (documentSnapshot.Exists)
                    {
                        Dictionary<string, object> city = documentSnapshot.ToDictionary();
                        string json = JsonConvert.SerializeObject(city);
                        RouteModel newuser = JsonConvert.DeserializeObject<RouteModel>(json);
                        newuser.RouteId = documentSnapshot.Id;
                        await documentSnapshot.Reference.DeleteAsync();
                    }
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
