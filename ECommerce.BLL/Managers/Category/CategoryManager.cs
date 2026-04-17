using ECommerce.BLL.Dtos.Category;
using ECommerce.Common.Errors.Category;
using ECommerce.Common.Pagination;
using ECommerce.Common.ResultPattern;
using ECommerce.DAL.Repositories.UnitOfWork;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace ECommerce.BLL.Managers.Category
{
    public class CategoryManager : ICategoryManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);

            if (category is null)
                return Result.Failure<CategoryResponse>(CategoryError.CategoryNotFound);

            var result = category.Adapt<CategoryResponse>();
            return Result.Success(result);
        }

        public async Task<Result<CategoryResponse>> AddCategory(CreateCategoryRequest createCategoryRequest)
        {
            var category = createCategoryRequest.Adapt<ECommerce.DAL.Entities.Category>();

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


