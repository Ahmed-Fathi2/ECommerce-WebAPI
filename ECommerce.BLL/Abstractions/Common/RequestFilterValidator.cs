using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.BLL.Abstractions.Common
{
    public class RequestFilterValidator:AbstractValidator<BaseRequestFilter>
    {
        public RequestFilterValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("PageNumber Must be greater than 0 tets");

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .WithMessage("PageSize Must be greater than 0")
                .LessThanOrEqualTo(50)
                .WithMessage("PageSize Must be Lower than or Equl 50");

            RuleFor(x => x.SearchValue)
                .MaximumLength(256)
                .WithMessage("SearchValue cannot exceed 256 characters")
                .When(x => x.SearchValue is not null);


            RuleFor(x => x.SortColumn)
                .MaximumLength(256)
                .WithMessage("SortColumn cannot exceed 256 characters")
                .When(x => x.SortColumn is not null);


            RuleFor(x => x.SortDirection)
                .Must(x => x == "ASC" || x == "DESC")
                .When(x => x.SortDirection is not null)
                .WithMessage("SortDirection must be ASC or DESC");




        }
    }
}

