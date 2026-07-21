using Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
   
        public class ProductValidator : AbstractValidator<CreateProductDto>
        {
            public ProductValidator()
            {
                RuleFor(x => x.ProductName)
                    .NotEmpty()
                    .MaximumLength(255);

                RuleFor(x => x.CreatedBy)
                    .NotEmpty()
                    .MaximumLength(100);
            }
        }
    }

