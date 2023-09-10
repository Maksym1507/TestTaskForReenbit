namespace TestTaskForReenbit.Data.Requests
{
    public class UploadFileRequest
    {
        public string Email { get; set; }

        public IFormFile FormFile { get; set; }
        
        public string FileName { get; set; }
    }
}
