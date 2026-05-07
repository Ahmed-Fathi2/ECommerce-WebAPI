using ECommerce.Application.DTOs;
using ECommerce.Application.Common.Pagination;
using ECommerce.Application.Common.Constants;
using ECommerce.Application.Common.ResultPattern;

namespace ECommerce.Application.Contracts
{
    public interface IProductService
    {

        //Task<Result<IEnumerable<ProductsResponse>>> GetAllProducts();

        Task<Result<PaginatedList<ProductsResponse>>> GetProducts(ProductRequestFilter requestFilter);
        Task<Result<ProductDetailsResponse>> GetProductById(Guid id);
        Task<Result<ProductsResponse>> AddProduct(CreateProductRequest createProductRequest);
        Task<Result> UpdateProduct(Guid id, UpdateProductRequest UpdateProductRequest);
        Task<Result> DeleteProduct(Guid id);

    }
}








