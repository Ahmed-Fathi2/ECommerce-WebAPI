namespace ECommerce.BLL.Dtos.Category
{
    public record CreateCategoryRequest
    (
        string Name,
        string Description,
        string? ImageUrl
    );
}
