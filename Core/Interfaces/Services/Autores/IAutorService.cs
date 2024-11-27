using Core.DTOs;
using Core.Entities;

namespace Core.Interfaces.Services.Autores;

public interface IAutorService
{
  Task<Autor> GetAutorAsync(Guid id);
  Task<GenericResponse<Autor>> CreateAutorAsync(Autor entity);
  Task<GenericResponse<Autor>> UpdateAutorAsync(int id, Autor entity);
  Task<GenericResponse<bool>> ExcludeAutorAsync(int id);
}
