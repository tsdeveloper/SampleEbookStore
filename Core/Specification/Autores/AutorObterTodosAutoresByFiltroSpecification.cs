using Core.Entities;
using Core.Specification.Autors.SpecParams;

namespace Core.Specification.Autors;

public class AutorObterTodosAutoresByFiltroSpecification : BaseSpecification<Autor>
{
  public AutorObterTodosAutoresByFiltroSpecification(AutorSpecParams specParams)
  : base(x =>
        (specParams.CodAu == null || x.CodAu.Equals(specParams.CodAu))
        && (specParams.Search == null || x.Nome.Contains(specParams.Search))
  )
  {
    AddOrderby(x => x.Nome);
    ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);

    if (!string.IsNullOrWhiteSpace(specParams.Sort))
    {
      switch (specParams)
      {
        case AutorSpecParams p when p.Sort.Equals("desc"):
          AddOrderByDescending(p => p.Nome);
          break;
        default:
          AddOrderby(p => p.Nome);
          break;
      }
    }

  }
}
