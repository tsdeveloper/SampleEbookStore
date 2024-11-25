using Core.DTOs;
using Core.Entities;

namespace Core.Interfaces.Services.Livros;

public interface ILivroService
{
  Task<Livro> GetLivroAsync(Guid id);
  Task<GenericResponse<Livro>> CreateLivroAsync(Livro entity);
  Task<GenericResponse<Livro>> UpdateLivroAsync(Guid uid, Livro entity);
  Task<GenericResponse<bool>> ExcludeLivroAsync(Guid id);
}
