using API.Controllers.Test.Builder;
using Core.Entities;

namespace API.Controllers.Test.Builder.Entities;

public class LivroBuilder : BaseBuilder<Livro>
{
    public LivroBuilder Default()
    {
        _instance.CodI = 1;
        _instance.Titulo = "Livro1";
        _instance.Editora = "BooktStore";
        _instance.Edicao = 1;
        _instance.AnoPublicacao = "2024";
        return this;

    }
}
