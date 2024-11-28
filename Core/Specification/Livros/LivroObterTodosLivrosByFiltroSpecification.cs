using Core.Entities;
using Core.Specification.Livros.SpecParams;

namespace Core.Specification.Livros;

public class LivroObterTodosLivrosByFiltroSpecification : BaseSpecification<Livro>
{
    public LivroObterTodosLivrosByFiltroSpecification(LivroSpecParams specParams)
    : base(x =>
          (specParams.CodL == null || x.CodL.Equals(specParams.CodL))
          && (specParams.Search == null || x.Titulo.Contains(specParams.Search))
          && (specParams.Editora == null || x.Editora.Contains(specParams.Editora))
          && (specParams.Edicao == null || x.Edicao.Equals(specParams.Edicao))
          && (specParams.AnoPublicacao == null || x.AnoPublicacao.Equals(specParams.AnoPublicacao))
    )
    {
        if (specParams.IncluirAutores)
            AddInclude(x => x.Livro_AssuntoList);

        if (specParams.IncluirAssuntos)
            AddInclude(x => x.Livro_AutorList);

        AddOrderby(x => x.Titulo);
        ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);

        if (!string.IsNullOrWhiteSpace(specParams.Sort))
        {
            switch (specParams)
            {
                case LivroSpecParams p when p.Sort.Equals("desc"):
                    AddOrderByDescending(p => p.Titulo);
                    break;
                default:
                    AddOrderby(p => p.Titulo);
                    break;
            }
        }

    }
}
