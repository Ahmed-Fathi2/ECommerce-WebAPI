using ECommerce.BLL.Abstractions.Common;
using ECommerce.BLL.Abstractions.Constants;
using ECommerce.BLL.Abstractions.ResultPattern;
using ECommerce.BLL.Dtos.Product;

namespace ECommerce.BLL.Managers.Product
{
    public interface IProductManager
    {

        Task<Result<IEnumerable<ProductsResponse>>> GetAllProducts();

        Task<Result<PaginatedList<ProductsResponse>>> GetProducts(ProductRequestFilter requestFilter);
        Task<Result<ProductDetailsResponse>> GetProductById(Guid id);
        Task<Result<ProductsResponse>> AddProduct(CreateProductRequest createProductRequest);
        Task<Result> UpdateProduct(int id, UpdateProductRequest UpdateProductRequest);
        Task<Result> DeleteProduct(int id);

    }
}


