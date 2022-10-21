namespace DeliveryVHGP.Infrastructure.Services
{
    public interface IFileService
    {
        //public Task<string> UploadFile(IFormFile file);
        Task<string> UploadFile(string fileImg ,string base64String);
    }
}
