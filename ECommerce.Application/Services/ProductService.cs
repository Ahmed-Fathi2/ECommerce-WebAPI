using ECommerce.Application.Common.Constants;
using ECommerce.Application.Common.Errors;
using ECommerce.Application.Common.Pagination;
using ECommerce.Application.Common.ResultPattern;
using ECommerce.Application.Contracts;
using ECommerce.Application.DTOs;
using ECommerce.Application.Mappings;
using ECommerce.Domain.Repositories;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace ECommerce.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBlobStorageService _blobStorageService;
        private readonly ICacheService _cacheService;

        public ProductService(IUnitOfWork unitOfWork , IBlobStorageService blobStorageService,ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _blobStorageService = blobStorageService;
            _cacheService = cacheService;
        }

        //public async Task<Result<IEnumerable<ProductsResponse>>> GetAllProducts()
        //{
        //    var products = await _unitOfWork.ProductRepository.GetAllAsync();

        //    var result = products.Adapt<IEnumerable<ProductsResponse>>();

        //    return Result.Success(result);


        //}

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
            var result = await PaginatedList<ProductsResponse>.CreateAsync(ProductsResponse, requestFilter.PageNumber, requestFilter.PageSize);

 

            return Result.Success(result);
        }
        public async Task<Result<ProductDetailsResponse>> GetProductById(Guid id)
        {
            string cacheKey = $"Product_{id}";


          var cachedProduct = await _cacheService.GetAsync<ProductDetailsResponse>(cacheKey);

            if (cachedProduct is not null)
                return Result.Success(cachedProduct);



            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);

            if (product is null)
                return Result.Failure<ProductDetailsResponse>(ProductError.ProductNotFound);

            var productDetailsResponse = product.Adapt<ProductDetailsResponse>();
            productDetailsResponse.ImageUrl = _blobStorageService.GetBlobSasUrl(product.ImageUrl);

            await _cacheService.SetAsync(cacheKey, productDetailsResponse);

            return Result.Success(productDetailsResponse);

        }
        public async Task<Result<ProductsResponse>> AddProduct(CreateProductRequest createProductRequest)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(createProductRequest.CategoryId);
            if (category is null)
                return Result.Failure<ProductsResponse>(CategoryError.CategoryNotFound);


            var product = createProductRequest.Adapt<ECommerce.Domain.Entities.Product>();

            _unitOfWork.ProductRepository.Add(product);
            await _unitOfWork.SaveAsync();


            var result = product.Adapt<ProductsResponse>();

            //result.ImageUrl = _blobStorageService.GetBlobSasUrl(product.ImageUrl);

            return Result.Success(result);
        }
        public async Task<Result> UpdateProduct(Guid id, UpdateProductRequest UpdateProductRequest)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product is null)
                return Result.Failure<ProductDetailsResponse>(ProductError.ProductNotFound);

            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(UpdateProductRequest.CategoryId);
            if (category is null)
                return Result.Failure<ProductsResponse>(CategoryError.CategoryNotFound);


            UpdateProductRequest.Adapt(product);

            await _unitOfWork.SaveAsync();

            await _cacheService.RemoveAsync($"Product_{id}");

            return Result.Success();
        }

        public async Task<Result> DeleteProduct(Guid id)
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














