using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.DTOs.Assuntos
{
    public class AssuntoReturnDto
    {
        public int CodAs { get; set; }
        public string Descricao { get; set; }
    }

    public class AssuntoCriarDto
    {        public string Descricao { get; set; }
    }

    public class AssuntoAtualizarDto
    {
     public int CodAs { get; set; }
        public string Descricao { get; set; }
    }
}