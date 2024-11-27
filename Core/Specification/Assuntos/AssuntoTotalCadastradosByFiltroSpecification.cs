using Core.Entities;
using Core.Specification.Assuntos.SpecParams;

namespace Core.Specification.Assuntos;

public class AssuntoTotalCadastradosByFiltroSpecification : BaseSpecification<Assunto>
{
  public AssuntoTotalCadastradosByFiltroSpecification(AssuntoSpecParams specParams)
  : base(x => 
         (specParams.CodAs == null || x.CodAs.Equals(specParams.CodAs))
        && (specParams.Search == null || x.Descricao.Contains(specParams.Search))
  )
  {
    
  }
}
