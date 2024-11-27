using Core.Entities;
using Core.Specification.Autors.SpecParams;

namespace Core.Specification.Autors;

public class AutorTotalCadastradosByFiltroSpecification : BaseSpecification<Autor>
{
  public AutorTotalCadastradosByFiltroSpecification(AutorSpecParams specParams)
  : base(x => 
         (specParams.CodAu == null || x.CodAu.Equals(specParams.CodAu))
        && (specParams.Search == null || x.Nome.Contains(specParams.Search))
  )
  {
    
  }
}
