using Core.Entities;
using Core.Specification.Assuntos.SpecParams;

namespace Core.Specification.Assuntos;

public class AssuntoObterTodosAssuntosByFiltroSpecification : BaseSpecification<Assunto>
{
  public AssuntoObterTodosAssuntosByFiltroSpecification(AssuntoSpecParams specParams)
  : base(x =>
        (specParams.CodAs == null || x.CodAs.Equals(specParams.CodAs))
        && (specParams.Search == null || x.Descricao.Contains(specParams.Search))
  )
  {
    AddOrderby(x => x.Descricao);
    ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);

    if (!string.IsNullOrWhiteSpace(specParams.Sort))
    {
      switch (specParams)
      {
        case AssuntoSpecParams p when p.Sort.Equals("desc"):
          AddOrderByDescending(p => p.Descricao);
          break;
        default:
          AddOrderby(p => p.Descricao);
          break;
      }
    }

  }
}
