using Microsoft.AspNetCore.Mvc;
using TestTaskForReenbit.Data.Requests;
using TestTaskForReenbit.Services.Abstractions;

namespace TestTaskForReenbit.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class BlobStorageController : ControllerBase
    {
        private readonly IBlobStorageService _blobStorageService;
        private readonly ILogger<BlobStorageController> _logger;

        public BlobStorageController(ILogger<BlobStorageController> logger, IBlobStorageService blobStorageService)
        {
            _logger = logger;
            _blobStorageService = blobStorageService;
        }

        [HttpPost]
        public async Task<IActionResult> AddFileToBlobStorage([FromForm] UploadFileRequest request)
        {
            var result = await _blobStorageService.AddFileToBlobStorageAsync(request.Email, request.FileName, request.FormFile);
            return Ok(result);
        }
    }
}