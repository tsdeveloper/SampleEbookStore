using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DTOs.Assuntos;
using FluentValidation;

namespace Core.Validators.Assuntos
{
    public class AssuntoCriarDtoValidator : AbstractValidator<AssuntoCriarDto>
    {
        public AssuntoCriarDtoValidator()
        {
            RuleFor(c => c.Descricao).NotEmpty().NotNull().MaximumLength(40).WithMessage("{PropertyName} is required.");
        }
    }

    public class AssuntoAtualizarDtoValidator : AbstractValidator<AssuntoAtualizarDto>
    {
        public AssuntoAtualizarDtoValidator()
        {
            RuleFor(c => c.CodAs).NotEmpty().NotNull().GreaterThan(0).WithMessage("{PropertyName} is required.");
            RuleFor(c => c.Descricao).NotEmpty().NotNull().MaximumLength(40).WithMessage("{PropertyName} is required.");

        }
    }
}