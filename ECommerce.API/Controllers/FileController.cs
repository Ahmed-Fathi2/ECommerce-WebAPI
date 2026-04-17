using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ECommerce.BLL.Abstractions;
using ECommerce.BLL.Dtos.Product;
using ECommerce.BLL.Managers.FileManager;

namespace ECommerce.API.Controllers
{
    [Route("api/image")]
    [ApiController]
    public class FileController(IFileManager fileManager, IWebHostEnvironment env) : ControllerBase
    {
        private readonly IFileManager _fileManager = fileManager;
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

