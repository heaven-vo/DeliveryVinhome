namespace DeliveryVHGP_WebApi.IRepositories
{
    public interface IFileService
    {
        public Task<string> UploadFile(IFormFile file);
        Task<string> UploadFile(string base64String);
    }
}
