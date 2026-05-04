using ECommerce.Application.Common.Errors;
using ECommerce.Application.Common.ResultPattern;

namespace ECommerce.Application.Common.Errors
{
    public static class CategoryError
    {
        public static readonly Error CategoryNotFound = new Error("Category.NotFound", "The category was not found.", 404);
    }
}




