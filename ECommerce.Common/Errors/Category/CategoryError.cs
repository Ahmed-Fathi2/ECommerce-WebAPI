using ECommerce.Common.ResultPattern;

namespace ECommerce.Common.Errors.Category
{
    public static class CategoryError
    {
        public static readonly Error CategoryNotFound = new Error("Category.NotFound", "The category was not found.", 404);
    }
}
