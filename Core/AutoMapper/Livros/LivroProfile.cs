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
                   .ForMember(x => x.Autores, o => o.MapFrom(d => d.Livro_AutorList))
                .ForMember(x => x.Assuntos, o => o.MapFrom(d => d.Livro_AssuntoList))
                ;

            CreateMap<LivroCriarDto, Livro>()
                .ForMember(x => x.Livro_AutorList, o => o.MapFrom(d => d.Autores))
                .ForMember(x => x.Livro_AssuntoList, o => o.MapFrom(d => d.Assuntos))
                ;

            CreateMap<LivroAtualizarDto, Livro>()
              .ForMember(x => x.Livro_AutorList, o => o.MapFrom(d => d.Autores))
                .ForMember(x => x.Livro_AssuntoList, o => o.MapFrom(d => d.Assuntos))
                ;

            CreateMap<LivroAutorCreateDto, Livro_Autor>()
                .ForMember(x => x.Autor_CodAu, o => o.MapFrom(d => d.CodAu))
                .ForMember(x => x.Livro_CodL, o => o.MapFrom(d => d.CodL))
                .ReverseMap()
               ;

            CreateMap<LivroAssuntoCreateDto, Livro_Assunto>()
               .ForMember(x => x.Assunto_CodAs, o => o.MapFrom(d => d.CodAs))
               .ForMember(x => x.Livro_CodL, o => o.MapFrom(d => d.CodL))
               .ReverseMap()
              ;

            CreateMap<LivroAutorReturnDto, Livro_Autor>()
             .ForMember(x => x.Autor_CodAu, o => o.MapFrom(d => d.CodAu))
             .ForMember(x => x.Livro_CodL, o => o.MapFrom(d => d.CodL))
             .ForMember(x => x.Autor, o => o.MapFrom(d => d.Autor))
             .ReverseMap()
            ;

            CreateMap<LivroAssuntoReturnDto, Livro_Assunto>()
               .ForMember(x => x.Assunto_CodAs, o => o.MapFrom(d => d.CodAs))
               .ForMember(x => x.Livro_CodL, o => o.MapFrom(d => d.CodL))
               .ForMember(x => x.Assunto, o => o.MapFrom(d => d.Assunto))
               .ReverseMap()
              ;
        }
    }
}