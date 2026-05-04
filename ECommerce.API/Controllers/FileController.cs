using ECommerce.Application.DTOs;
using ECommerce.Application.Common.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ECommerce.Application.Common;
using ECommerce.Application.DTOs;
using ECommerce.Application.Contracts;

namespace ECommerce.API.Controllers
{
    [Route("api/image")]
    [ApiController]
    [Authorize(Policy = "RequireAdmin")]
    public class FileController(IFileService FileService, IWebHostEnvironment env) : ControllerBase
    {
        private readonly IFileService _fileManager = FileService;
        private readonly IWebHostEnvironment _env = env;

        [HttpPost("Upload")]
        public async Task<ActionResult> UploadFile([FromForm]UploadProductImageRequest file)
        {
            var schema = Request.Scheme;
            var host = Request.Host.Value;
            var baseUrl = _env.WebRootPath;

           var result= await _fileManager.UploadFileAsync(file, baseUrl, schema, host);

            return result.IsSuccess? Ok(result.Value): result.ToProblem();
        }
    }
}







