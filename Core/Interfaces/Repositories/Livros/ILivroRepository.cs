using Core.Entities;
using Core.Specification.Livros;

namespace Core.Interfaces.Repositories.Livros;

public interface ILivroRepository : IGenericRepository<Livro>
{
    Task<bool> AdicionarLivroAutorRelacionado(List<Livro_Autor> novosLivroAutores);
    Task<bool> RemoverLivroAutorRelacionado(List<Livro_Autor> excluidoLivroAutores);
    Task<bool> AdicionarLivro_AssuntoRelacionado(List<Livro_Assunto> livro_Assuntos);
    Task<bool> RemoverLivro_AssuntoRelacionado(List<Livro_Assunto> livro_Assuntos);
    Task<IReadOnlyList<LivroRelatorio>> GetTodosLivrosByView();
}
