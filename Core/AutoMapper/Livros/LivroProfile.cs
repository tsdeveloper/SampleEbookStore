using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.DTOs.Livros;
using Core.Entities;

namespace Core.AutoMapper.Livros
{
    public class LivroProfile : Profile
    {
        public LivroProfile()
        {
            CreateMap<Livro, LivroReturnDto>()
                .ReverseMap();

            CreateMap<Livro, LivroCriarDto>()
                .ReverseMap();

            CreateMap<Livro, LivroAtualizarDto>()
                .ReverseMap();
        }
    }
}