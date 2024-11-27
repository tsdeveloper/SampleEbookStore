using Core.Entities;
using Core.Specification.Livros.SpecParams;

namespace Core.Specification.Livros;

public class LivroTotalCadastradosByFiltroSpecification : BaseSpecification<Livro>
{
  public LivroTotalCadastradosByFiltroSpecification(LivroSpecParams specParams)
  : base(x => 
        (specParams.CodI == null || x.CodI.Equals(specParams.CodI))
        && (specParams.Search == null || x.Titulo.Contains(specParams.Search))
        && (specParams.Editora == null || x.Editora.Contains(specParams.Editora))
        && (specParams.Edicao == null || x.Edicao.Equals(specParams.Edicao))
        && (specParams.AnoPublicacao == null || x.AnoPublicacao.Equals(specParams.AnoPublicacao))
  )
  {
    
  }
}
