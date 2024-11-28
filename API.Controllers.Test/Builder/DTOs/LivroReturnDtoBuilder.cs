using Core.DTOs.Livros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers.Test.Builder.DTOs
{
    public class LivroReturnDtoBuilder : BaseBuilder<LivroReturnDto>
    {
        public LivroReturnDtoBuilder Default()
        {
            _instance.CodL = 1;
            _instance.Titulo = "Livro1";
            _instance.Editora = "BooktStore";
            _instance.Edicao = 1;
            _instance.AnoPublicacao = "2024";
            return this;

        }
    }
}
