using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ECommerce.BLL.Abstractions;
using ECommerce.BLL.Abstractions.Constants;
using ECommerce.BLL.Dtos.Product;
using ECommerce.BLL.Managers.Product;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductManager _productManager;

        public ProductController(IProductManager productManager)
        {
            _productManager = productManager;
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<ProductsResponse>>> GetAllProducts()
        //{
        //    var result = await _productManager.GetAllProducts();
        //    return Ok(result.Value);
        //}

        [HttpGet]
        //[Authorize]
        public async Task<ActionResult<PaginatedList<ProductsResponse>>> GetAllProducts([FromQuery] ProductRequestFilter requestFilter)
        {
            var result = await _productManager.GetProducts(requestFilter);
            //throw new Exception("Error");
            return Ok(result.Value);
        }


        [HttpGet("{id}")]
        [Authorize(Roles =DefaultRole.Admin)]
        public async Task<ActionResult<ProductsResponse>> GetProductById([FromRoute] int id)
        {
            var result = await _productManager.GetProductById(id);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();

        }

        [HttpPost]
        public async Task<ActionResult> AddProduct([FromBody] CreateProductRequest CreateProductRequest)
        {
            var result = await _productManager.AddProduct(CreateProductRequest);
            return CreatedAtAction("GetProductById", new { id = result.Value.Id }, result.Value);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct([FromRoute] int id, [FromBody] UpdateProductRequest UpdateProductRequest)
        {
            var result = await _productManager.UpdateProduct(id, UpdateProductRequest);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct([FromRoute] int id)
        {
            var result = await _productManager.DeleteProduct(id);
            return result.IsSuccess ? NoContent() : result.ToProblem();

        }
    }
}

