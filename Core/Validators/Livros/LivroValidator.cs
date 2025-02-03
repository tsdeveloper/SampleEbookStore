using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DTOs.Livros;
using FluentValidation;

namespace Core.Validators.Livros
{
    public class LivroCriarDtoValidator : AbstractValidator<LivroCriarDto>
    {
        public LivroCriarDtoValidator()
        {
            RuleFor(c => c.Titulo).NotEmpty().NotNull().MaximumLength(40).WithMessage("{PropertyName} is required.");
            RuleFor(c => c.Editora).NotEmpty().NotNull().MaximumLength(40).WithMessage("{PropertyName} is required.");
            RuleFor(c => c.Edicao).NotEmpty().NotNull().WithMessage("{PropertyName} is required.");
            RuleFor(c => c.AnoPublicacao).NotEmpty().NotNull().GreaterThan(4).WithMessage("{PropertyName} is required.");
            RuleFor(c => c.Preco).NotEmpty().NotNull().WithMessage("{PropertyName} is required.");
        }
    }

    public class LivroAtualizarDtoValidator : AbstractValidator<LivroAtualizarDto>
    {
        public LivroAtualizarDtoValidator()
        {
            RuleFor(c => c.CodL).NotEmpty().NotNull().WithMessage("{PropertyName} is required.");
            RuleFor(c => c.Titulo).NotEmpty().NotNull().MaximumLength(40).WithMessage("{PropertyName} is required.");
            RuleFor(c => c.Editora).NotEmpty().NotNull().MaximumLength(40).WithMessage("{PropertyName} is required.");
            RuleFor(c => c.Edicao).NotEmpty().NotNull().WithMessage("{PropertyName} is required.");
            RuleFor(c => c.AnoPublicacao).NotEmpty().NotNull().GreaterThan(4).WithMessage("{PropertyName} is required.");
            RuleFor(c => c.Preco).NotEmpty().NotNull().WithMessage("{PropertyName} is required.");

        }
    }
}