using ECommerce.Application.DTOs;
using ECommerce.Application.Common.Pagination;
using ECommerce.Application.Common.Constants;
using ECommerce.Application.Common.ResultPattern;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Application.Contracts
{
    public interface ICategoryService
    {
        Task<Result<IEnumerable<CategoryResponse>>> GetAllCategories();
        Task<Result<PaginatedList<CategoryResponse>>> GetCategories(CategoryRequestFilter requestFilter);
        Task<Result<CategoryResponse>> GetCategoryById(Guid id);
        Task<Result<CategoryResponse>> AddCategory(CreateCategoryRequest createCategoryRequest);
        Task<Result> UpdateCategory(Guid id, UpdateCategoryRequest updateCategoryRequest);
        Task<Result> DeleteCategory(Guid id);
    }
}






