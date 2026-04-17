using FluentValidation;
using ECommerce.BLL.Dtos.Product;

namespace ECommerce.BLL.Validation.Product
{
    public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductRequestValidator()
        {
            RuleFor(x => x.Name)
               .NotEmpty().WithMessage("Name Property  is required .")
               .Length(3, 100).WithMessage("Name must be between 3 and 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(256).WithMessage("Description cannot exceed 256 characters.");

            RuleFor(x => x.Price)
           .GreaterThan(0).WithMessage("Price must be greater than 0");

            RuleFor(x => x.Count)
                .GreaterThanOrEqualTo(0).WithMessage("Count cannot be negative");

        }
    }
}

