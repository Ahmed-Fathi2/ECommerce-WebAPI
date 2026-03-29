using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using ECommerce.BLL.Abstractions;
using ECommerce.BLL.Abstractions.Errors.Product;
using ECommerce.BLL.Dtos.Product;
using ECommerce.DAL.Repositories.UnitOfWork;
using ECommerce.DAL;

namespace ECommerce.BLL.Managers.Product
{
    public class ProductManager : IProductManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IEnumerable<ProductsResponse>>> GetAllProducts()
        {
            var products = await _unitOfWork.ProductRepository.GetAllAsync();

            var result = products.Adapt<IEnumerable<ProductsResponse>>();

            return Result.Success(result);


        }

        public async Task<Result<PaginatedList<ProductsResponse>>> GetProducts(ProductRequestFilter requestFilter)
        {
            var query = await _unitOfWork.ProductRepository.GetAllProducts();

            //Search
            if (string.IsNullOrEmpty(requestFilter.SearchValue) is false)
                query = query.Where(p => p.Name.Contains(requestFilter.SearchValue)
                                 || p.Description.Contains(requestFilter.SearchValue));

            //Sorting
            if (string.IsNullOrEmpty(requestFilter.SortColumn) is false)
                query = query.OrderBy($"{requestFilter.SortColumn} {requestFilter.SortDirection}");

            //Filter
            if (requestFilter.CategoryId.HasValue)
                query = query.Where(p => p.CategoryId == requestFilter.CategoryId);

            if (requestFilter.MinPrice.HasValue)
                query = query.Where(p => p.Price >= requestFilter.MinPrice);

            if (requestFilter.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= requestFilter.MaxPrice);


            //Mapping
            var ProductsResponse = query.ProjectToType<ProductsResponse>()
                                           .AsNoTracking();

            //Pagination
            var result = await PaginatedList<ProductsResponse>
                            .CreateAsync(ProductsResponse, requestFilter.PageNumber, requestFilter.PageSize);

            return Result.Success(result);
        }
        public async Task<Result<ProductDetailsResponse>> GetProductById(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetProductByCategoryAsync(id);

            if (product is null)
                return Result.Failure<ProductDetailsResponse>(ProductError.ProductNotFound);

            var result = product.Adapt<ProductDetailsResponse>();

            return Result.Success(result);

        }
        public async Task<Result<ProductsResponse>> AddProduct(CreateProductRequest createProductRequest)
        {

            var product = createProductRequest.Adapt<ECommerce.DAL.Product>();

            _unitOfWork.ProductRepository.Add(product);
            await _unitOfWork.SaveAsync();


            var result = product.Adapt<ProductsResponse>();
            return Result.Success(result);
        }
        public async Task<Result> UpdateProduct(int id, UpdateProductRequest UpdateProductRequest)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product is null)
                return Result.Failure<ProductDetailsResponse>(ProductError.ProductNotFound);

            UpdateProductRequest.Adapt(product);

            await _unitOfWork.SaveAsync();

            return Result.Success();
        }

        public async Task<Result> DeleteProduct(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product is null)
                return Result.Failure<ProductDetailsResponse>(ProductError.ProductNotFound);

            _unitOfWork.ProductRepository.Delete(product);
            await _unitOfWork.SaveAsync();

            return Result.Success();
        }

      
    }
}



