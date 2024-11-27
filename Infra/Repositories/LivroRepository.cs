using Core.Entities;
using Core.Interfaces.Repositories.Livros;
using Infra.Data;

namespace Infra.Repositories;

public class LivroRepository : GenericRepository<Livro>, ILivroRepository
{
    public LivroRepository(SampleEbookStoreContext context) : base(context)
    {
    }
}
