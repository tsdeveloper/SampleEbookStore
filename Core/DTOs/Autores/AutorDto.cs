using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.DTOs.Autors
{
    public class AutorReturnDto
    {
        public int CodAu { get; set; }
        public string Nome { get; set; }
    }

    public class AutorCriarDto
    {        public string Nome { get; set; }
    }

    public class AutorAtualizarDto
    {
     public int CodAu { get; set; }
        public string Nome { get; set; }
    }
}