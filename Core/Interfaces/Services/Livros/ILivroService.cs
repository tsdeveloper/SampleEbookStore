using Core.DTOs;
using Core.Entities;

namespace Core.Interfaces.Services.Livros;

public interface ILivroService
{
  Task<Livro> GetLivroAsync(Guid id);
  Task<GenericResponse<Livro>> CreateLivroAsync(Livro entity);
  Task<GenericResponse<Livro>> UpdateLivroAsync(int id, Livro entity);
  Task<GenericResponse<bool>> ExcludeLivroAsync(int id);
}
