using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.DTOs.Assuntos;
using Core.Entities;

namespace Core.AutoMapper.Assuntos
{
    public class AssuntoProfile : Profile
    {
        public AssuntoProfile()
        {
            CreateMap<Assunto, AssuntoReturnDto>()
                .ReverseMap();

            CreateMap<Assunto, AssuntoCriarDto>()
                .ReverseMap();

            CreateMap<Assunto, AssuntoAtualizarDto>()
                .ReverseMap();
        }
    }
}