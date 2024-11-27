using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.DTOs.Livros
{
    public class LivroReturnDto
    {
        public int CodI { get; set; }
        public string Titulo { get; set; }
        public string Editora { get; set; }
        public int Edicao { get; set; }
        public string AnoPublicacao { get; set; }
        // public ICollection<Livro_Autor> Livro_AutorList { get; set; } = new List<Livro_Autor>();
        // public ICollection<Livro_Assunto> Livro_AssuntoList { get; set; } = new List<Livro_Assunto>();
    }

    public class LivroCriarDto
    {
        public string Titulo { get; set; }
        public string Editora { get; set; }
        public int Edicao { get; set; }
        public string AnoPublicacao { get; set; }
    }

    public class LivroAtualizarDto
    {
        public int CodI { get; set; }
        public string Titulo { get; set; }
        public string Editora { get; set; }
        public int Edicao { get; set; }
        public string AnoPublicacao { get; set; }
    }
}