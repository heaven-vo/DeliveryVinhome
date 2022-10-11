namespace DeliveryVHGP_WebApi.IRepositories
{
    public interface IFileService
    {
        //public Task<string> UploadFile(IFormFile file);
        Task<string> UploadFile(string fileImg ,string base64String);
    }
}
