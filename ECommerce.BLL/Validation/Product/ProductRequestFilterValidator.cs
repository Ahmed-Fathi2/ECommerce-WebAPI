using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.BLL.Dtos.Product;

namespace ECommerce.BLL.Validation.Product
{
    public class ProductRequestFilterValidator:AbstractValidator<ProductRequestFilter>
    {
        public ProductRequestFilterValidator()
        {


            //RuleFor(x => x.CategoryId)
            //    .GreaterThan(0)
            //    .WithMessage("{PropertyName} must be greater than 0")
            //    .When(x => x.CategoryId.HasValue);

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

