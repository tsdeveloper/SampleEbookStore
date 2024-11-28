using Core.DTOs.Assuntos;
using Core.DTOs.Autors;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.DTOs.Livros
{
    public class LivroReturnDto
    {
        public int CodL { get; set; }
        public string Titulo { get; set; }
        public string Editora { get; set; }
        public int Edicao { get; set; }
        public int AnoPublicacao { get; set; }
        public ICollection<LivroAutorReturnDto> Autores { get; set; } = new List<LivroAutorReturnDto>();
        public ICollection<LivroAssuntoReturnDto> Assuntos { get; set; } = new List<LivroAssuntoReturnDto>();

    }

    public class LivroCriarDto
    {
        public string Titulo { get; set; }
        public string Editora { get; set; }
        public int Edicao { get; set; }
        public int AnoPublicacao { get; set; }
        public ICollection<LivroAutorCreateDto> Autores { get; set; } = new List<LivroAutorCreateDto>();
        public ICollection<LivroAssuntoCreateDto> Assuntos { get; set; } = new List<LivroAssuntoCreateDto>();
    }

    public class LivroAtualizarDto
    {
        public int CodL { get; set; }
        public string Titulo { get; set; }
        public string Editora { get; set; }
        public int Edicao { get; set; }
        public int AnoPublicacao { get; set; }
        public ICollection<LivroAutorCreateDto> Autores { get; set; } = new List<LivroAutorCreateDto>();
        public ICollection<LivroAssuntoCreateDto> Assuntos { get; set; } = new List<LivroAssuntoCreateDto>();
    }

    public class LivroAutorReturnDto
    {
        public int CodL { get; set; }
        public int CodAu { get; set; }
        public AutorReturnDto Autor { get; set; }

    }

    public class LivroAutorCreateDto
    {
        public int CodL { get; set; }
        public int CodAu { get; set; }

    }

    public class LivroAssuntoReturnDto
    {
        public int CodL { get; set; }
        public int CodAs { get; set; }
        public AssuntoReturnDto Assunto { get; set; }

    }

    public class LivroAssuntoCreateDto
    {
        public int CodL { get; set; }
        public int CodAs { get; set; }
    }
}