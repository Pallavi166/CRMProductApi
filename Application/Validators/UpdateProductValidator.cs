using Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
        public class UpdateProductValidator : AbstractValidator<UpdateProductDto>
        {
            public UpdateProductValidator()
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0);

                RuleFor(x => x.ProductName)
                    .NotEmpty()
                    .MaximumLength(255);

                RuleFor(x => x.ModifiedBy)
                    .NotEmpty()
                    .MaximumLength(100);
            }
        }
    }

