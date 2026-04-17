using ECommerce.Common.Pagination;
using ECommerce.Common.Constants;
using ECommerce.Common.ResultPattern;
using ECommerce.BLL.Dtos.Category;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.BLL.Managers.Category
{
    public interface ICategoryManager
    {
        Task<Result<IEnumerable<CategoryResponse>>> GetAllCategories();
        Task<Result<PaginatedList<CategoryResponse>>> GetCategories(CategoryRequestFilter requestFilter);
        Task<Result<CategoryResponse>> GetCategoryById(Guid id);
        Task<Result<CategoryResponse>> AddCategory(CreateCategoryRequest createCategoryRequest);
        Task<Result> UpdateCategory(Guid id, UpdateCategoryRequest updateCategoryRequest);
        Task<Result> DeleteCategory(Guid id);
    }
}
