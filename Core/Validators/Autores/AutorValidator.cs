using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DTOs.Autors;
using FluentValidation;

namespace Core.Validators.Autors
{
    public class AutorCriarDtoValidator : AbstractValidator<AutorCriarDto>
    {
        public AutorCriarDtoValidator()
        {
            RuleFor(c => c.Nome).NotEmpty().NotNull().MaximumLength(40).WithMessage("{PropertyName} is required.");
        }
    }

    public class AutorAtualizarDtoValidator : AbstractValidator<AutorAtualizarDto>
    {
        public AutorAtualizarDtoValidator()
        {
            RuleFor(c => c.CodAu).NotEmpty().NotNull().GreaterThan(0).WithMessage("{PropertyName} is required.");
            RuleFor(c => c.Nome).NotEmpty().NotNull().MaximumLength(40).WithMessage("{PropertyName} is required.");

        }
    }
}