namespace ECommerce.BLL.Dtos.Category
{
    public record UpdateCategoryRequest
    (
        string Name,
        string Description,
        string? ImageUrl
    );
}
