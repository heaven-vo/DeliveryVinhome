namespace DeliveryVHGP.Infrastructure.Services
{
    public interface ITimeStageService
    {
        Task<string> GetTime();
    }
}
