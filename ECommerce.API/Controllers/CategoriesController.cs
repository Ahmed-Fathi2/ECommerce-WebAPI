using ECommerce.BLL.Abstractions;
using ECommerce.Common.Constants;
using ECommerce.Common.Pagination;
using ECommerce.BLL.Dtos.Category;
using ECommerce.BLL.Dtos.Product;
using ECommerce.BLL.Managers.Category;
using ECommerce.BLL.Managers.FileManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(ICategoryManager categoryManager, IFileManager fileManager, IWebHostEnvironment env) : ControllerBase
    {
        private readonly ICategoryManager _categoryManager = categoryManager;
        private readonly IFileManager _fileManager = fileManager;
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

