using System.ComponentModel.DataAnnotations;

namespace TestTaskForReenbit.Data.Requests
{
    public class UploadFileRequest
    {
        [Required(ErrorMessage = "Email can't be empty")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "File must be selected")]
        public IFormFile FormFile { get; set; } = null!;

        [Required(ErrorMessage = "File name can't be empty")]
        [FileExtensions(Extensions = ".docx", ErrorMessage = "Only .docx files are allowed.")]
        public string FileName { get; set; } = null!;
    }
}
