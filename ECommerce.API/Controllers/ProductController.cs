using ECommerce.Application.DTOs;
using ECommerce.Application.Common.Settings;
using ECommerce.Application.Common;
using ECommerce.Application.Common.Constants;
using ECommerce.Application.Common.Pagination;
using ECommerce.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ECommerce.Application.Contracts;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProductService ProductService, IFileService FileService, IWebHostEnvironment env) : ControllerBase
    {
        private readonly IProductService _productManager = ProductService;
        private readonly IFileService _fileManager = FileService;
        private readonly IWebHostEnvironment _env = env;

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PaginatedList<ProductsResponse>>> GetAllProducts([FromQuery] ProductRequestFilter requestFilter)
        {
            var result = await _productManager.GetProducts(requestFilter);
            return Ok(result.Value);
        }


        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ProductDetailsResponse>> GetProductById([FromRoute] Guid id)
        {
            var result = await _productManager.GetProductById(id);

            return result.IsSuccess ? Ok(result.Value) :
                result.ToProblem();

        }

        [HttpPost]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<ActionResult> AddProduct([FromBody] CreateProductRequest CreateProductRequest)
        {
            var result = await _productManager.AddProduct(CreateProductRequest);

            if(!result.IsSuccess)
                return result.ToProblem();

            return CreatedAtAction("GetProductById", new { id = result.Value.Id }, result.Value);
        }


        [HttpPut("{id}")]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<ActionResult> UpdateProduct([FromRoute] Guid id, [FromBody] UpdateProductRequest UpdateProductRequest)
        {
            var result = await _productManager.UpdateProduct(id, UpdateProductRequest);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<ActionResult> DeleteProduct([FromRoute] Guid id)
        {
            var result = await _productManager.DeleteProduct(id);
            return result.IsSuccess ? NoContent() : result.ToProblem();

        }

        [HttpPost("{id}/image")]
        [Authorize(Policy = "RequireAdmin")]
        public async Task<ActionResult> UploadImage([FromRoute] Guid id, [FromForm] UploadProductImageRequest file)
        {
            var productResult = await _productManager.GetProductById(id);
            if (!productResult.IsSuccess) return productResult.ToProblem();

            var schema = Request.Scheme;
            var host = Request.Host.Value;
            var baseUrl = _env.WebRootPath;

            var uploadResult = await _fileManager.UploadFileAsync(file, baseUrl, schema, host);
            if (!uploadResult.IsSuccess) return uploadResult.ToProblem();

            var updateReq = new UpdateProductRequest(productResult.Value.Name, productResult.Value.Description, productResult.Value.Price, productResult.Value.Count, productResult.Value.CategoryId, uploadResult.Value);

            var updateResult = await _productManager.UpdateProduct(id, updateReq);

            return updateResult.IsSuccess ? Ok(new { ImageUrl = uploadResult.Value }) : updateResult.ToProblem();
        }
    }
}









