using ECommerce.Common.Pagination;
using ECommerce.Common.Constants;
using ECommerce.Common.ResultPattern;
using ECommerce.BLL.Dtos.Product;

namespace ECommerce.BLL.Managers.Product
{
    public interface IProductManager
    {

        Task<Result<IEnumerable<ProductsResponse>>> GetAllProducts();

        Task<Result<PaginatedList<ProductsResponse>>> GetProducts(ProductRequestFilter requestFilter);
        Task<Result<ProductDetailsResponse>> GetProductById(Guid id);
        Task<Result<ProductsResponse>> AddProduct(CreateProductRequest createProductRequest);
        Task<Result> UpdateProduct(Guid id, UpdateProductRequest UpdateProductRequest);
        Task<Result> DeleteProduct(Guid id);

    }
}


