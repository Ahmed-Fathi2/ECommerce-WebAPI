using ECommerce.BLL.Dtos.Product;
using ECommerce.Common.Pagination;
using FluentValidation;

namespace ECommerce.BLL.Validation.Product
{
    public class ProductRequestFilterValidator : AbstractValidator<ProductRequestFilter>
    {
        public ProductRequestFilterValidator()
        {


            Include(new RequestFilterValidator());
       
            RuleFor(x => x.MinPrice)
                .GreaterThanOrEqualTo(0)
                .WithMessage("{PropertyName} must be >= 0")
                .When(x => x.MinPrice.HasValue);

            RuleFor(x => x.MaxPrice)
                .GreaterThanOrEqualTo(0)
                .WithMessage("{PropertyName} must be >= 0")
                .When(x => x.MaxPrice.HasValue);


            RuleFor(x => x)
                .Must(x => !x.MinPrice.HasValue || !x.MaxPrice.HasValue || x.MinPrice <= x.MaxPrice)
                .WithMessage("MinPrice cannot be greater than MaxPrice");
        }
    }
}

