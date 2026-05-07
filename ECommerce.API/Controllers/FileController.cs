using ECommerce.Application.Common;
using ECommerce.Application.Contracts;
using ECommerce.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/image")]
    [ApiController]
    //[Authorize(Policy = "RequireAdmin")]
    public class FileController(IFileService FileService, IWebHostEnvironment env , IBlobStorageService blobStorageService) : ControllerBase
    {
        private readonly IFileService _fileManager = FileService;
        private readonly IWebHostEnvironment _env = env;
        private readonly IBlobStorageService _blobStorageService = blobStorageService;

        [HttpPost("Upload")]
        public async Task<ActionResult> UploadFile([FromForm] UploadProductImageRequest file)
        {
            var schema = Request.Scheme;
            var host = Request.Host.Value;
            var baseUrl = _env.WebRootPath;

            var result = await _fileManager.UploadFileAsync(file, baseUrl, schema, host);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPost("UploadToBlob")]
        public async Task<ActionResult> UploadToBlob([FromForm] UploadProductImageRequest file)
        {
            using var stream = file.File.OpenReadStream();

            var blobUrl = await _blobStorageService.UploadToBlobAsync(stream, file.File.FileName, file.File.ContentType);

            return Ok(new { Url = blobUrl });
        }
    }
}







