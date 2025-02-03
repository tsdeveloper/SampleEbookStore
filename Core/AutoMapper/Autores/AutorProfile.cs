using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.DTOs.Autors;
using Core.Entities;

namespace Core.AutoMapper.Autors
{
    public class AutorProfile : Profile
    {
        public AutorProfile()
        {
            CreateMap<Autor, AutorReturnDto>()
                .ReverseMap();

            CreateMap<Autor, AutorCriarDto>()
                .ReverseMap();

            CreateMap<Autor, AutorAtualizarDto>()
                .ReverseMap();
        }
    }
}