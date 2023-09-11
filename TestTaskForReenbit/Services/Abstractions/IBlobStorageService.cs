namespace TestTaskForReenbit.Services.Abstractions
{
    public interface IBlobStorageService
    {
        Task<string> AddFileToBlobStorageAsync(string email, string fileName, IFormFile formFile);
    }
}
