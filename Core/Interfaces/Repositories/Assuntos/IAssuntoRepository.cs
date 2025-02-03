using Core.Entities;

namespace Core.Interfaces.Repositories.Livros;

public interface IAssuntoRepository : IGenericRepository<Assunto>
{
    Task<List<Assunto>> GetListAllAssuntosByIds(List<Livro_Assunto> assuntos);
}
