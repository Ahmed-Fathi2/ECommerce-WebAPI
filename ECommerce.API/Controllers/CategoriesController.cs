using ECommerce.Application.DTOs;
using ECommerce.Application.Common.Settings;
using ECommerce.Application.Common;
using ECommerce.Application.Common.Constants;
using ECommerce.Application.Common.Pagination;
using ECommerce.Application.DTOs;
using ECommerce.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ECommerce.Application.Contracts;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(ICategoryService CategoryService, IFileService FileService, IWebHostEnvironment env) : ControllerBase
    {
        private readonly ICategoryService _categoryManager = CategoryService;
        private readonly IFileService _fileManager = FileService;
        private readonly IWebHostEnvironment _env = env;

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PaginatedList<CategoryResponse>>> GetAllCategories([FromQuery] CategoryRequestFilter requestFilter)
        {
            var result = await _categoryManager.GetCategories(requestFilter);
            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<CategoryResponse>> GetCategoryById([FromRoute] Guid id)
        {
            var result = await _categoryManager.GetCategoryById(id);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPost]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<ActionResult> AddCategory([FromBody] CreateCategoryRequest createCategoryRequest)
        {
            var result = await _categoryManager.AddCategory(createCategoryRequest);
            return result.IsSuccess ? CreatedAtAction("GetCategoryById", new { id = result.Value.Id }, result.Value) : result.ToProblem();
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<ActionResult> UpdateCategory([FromRoute] Guid id, [FromBody] UpdateCategoryRequest updateCategoryRequest)
        {
            var result = await _categoryManager.UpdateCategory(id, updateCategoryRequest);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<ActionResult> DeleteCategory([FromRoute] Guid id)
        {
            var result = await _categoryManager.DeleteCategory(id);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpPost("{id}/image")]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<ActionResult> UploadImage([FromRoute] Guid id, [FromForm] UploadProductImageRequest file)
        {
            var categoryResult = await _categoryManager.GetCategoryById(id);
            if (!categoryResult.IsSuccess) return categoryResult.ToProblem();

            var schema = Request.Scheme;
            var host = Request.Host.Value;
            var baseUrl = _env.WebRootPath;

            var uploadResult = await _fileManager.UploadFileAsync(file, baseUrl, schema, host);
            if (!uploadResult.IsSuccess) return uploadResult.ToProblem();

            var updateReq = new UpdateCategoryRequest(categoryResult.Value.Name, categoryResult.Value.Description, uploadResult.Value);
            var updateResult = await _categoryManager.UpdateCategory(id, updateReq);

            return updateResult.IsSuccess ? Ok(new { ImageUrl = uploadResult.Value }) : updateResult.ToProblem();
        }
    }
}








