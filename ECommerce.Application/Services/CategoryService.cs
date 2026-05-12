using ECommerce.Application.Common.Errors;
using ECommerce.Application.Mappings;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Repositories;
using ECommerce.Application.Common.Pagination;
using ECommerce.Application.Common.ResultPattern;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using ECommerce.Application.Contracts;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(IUnitOfWork unitOfWork , IMemoryCache memoryCache ,ILogger<CategoryService> logger)
        {
            _unitOfWork = unitOfWork;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<CategoryResponse>>> GetAllCategories()
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync();
            var result = categories.Adapt<IEnumerable<CategoryResponse>>();
            return Result.Success(result);
        }

        public async Task<Result<PaginatedList<CategoryResponse>>> GetCategories(CategoryRequestFilter requestFilter)
        {
            var query = await _unitOfWork.CategoryRepository.GetAllCategories();

            //Search
            if (!string.IsNullOrEmpty(requestFilter.SearchValue))
            {
                query = query.Where(c => c.Name.Contains(requestFilter.SearchValue)
                                      || c.Description.Contains(requestFilter.SearchValue));
            }

            //Sorting
            if (!string.IsNullOrEmpty(requestFilter.SortColumn))
            {
                query = query.OrderBy($"{requestFilter.SortColumn} {requestFilter.SortDirection}");
            }

            //Mapping
            var categoryResponses = query.ProjectToType<CategoryResponse>().AsNoTracking();

            //Pagination
            var result = await PaginatedList<CategoryResponse>.CreateAsync(categoryResponses, requestFilter.PageNumber, requestFilter.PageSize);

            return Result.Success(result);
        }

        public async Task<Result<CategoryResponse>> GetCategoryById(Guid id)
        {
            string cacheKey = $"Category_{id}";

            var category = await _memoryCache.GetOrCreateAsync(
                cacheKey,
                entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(10));

                    _logger.LogInformation($"Cache MISS : Going to DB, Key: {cacheKey}");

                    return _unitOfWork.CategoryRepository.GetByIdAsync(id);
                });

            if (category is null)
                return Result.Failure<CategoryResponse>(CategoryError.CategoryNotFound);

            var result = category.Adapt<CategoryResponse>();
            return Result.Success(result);
        }

        public async Task<Result<CategoryResponse>> AddCategory(CreateCategoryRequest createCategoryRequest)
        {
            var category = createCategoryRequest.Adapt<ECommerce.Domain.Entities.Category>();

            _unitOfWork.CategoryRepository.Add(category);
            await _unitOfWork.SaveAsync();

            var result = category.Adapt<CategoryResponse>();
            return Result.Success(result);
        }

        public async Task<Result> UpdateCategory(Guid id, UpdateCategoryRequest updateCategoryRequest)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (category is null)
                return Result.Failure(CategoryError.CategoryNotFound);

            updateCategoryRequest.Adapt(category);

            await _unitOfWork.SaveAsync();

            _memoryCache.Remove($"Category_{id}");

            return Result.Success();
        }

        public async Task<Result> DeleteCategory(Guid id)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (category is null)
                return Result.Failure(CategoryError.CategoryNotFound);

            _unitOfWork.CategoryRepository.Delete(category);
            await _unitOfWork.SaveAsync();

            return Result.Success();
        }
    }
}











