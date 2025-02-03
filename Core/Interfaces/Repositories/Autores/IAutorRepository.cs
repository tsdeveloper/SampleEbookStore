using Core.Entities;

namespace Core.Interfaces.Repositories.Livros;

public interface IAutorRepository : IGenericRepository<Autor>
{
    Task<List<Autor>> GetListAllAutoresByIds(List<Livro_Autor> autores);
}
