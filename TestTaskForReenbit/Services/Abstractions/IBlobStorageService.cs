namespace TestTaskForReenbit.Services.Abstractions
{
    public interface IBlobStorageService
    {
        Task<bool> AddFileToBlobStorageAsync(string email, string fileName, IFormFile formFile);
    }
}
