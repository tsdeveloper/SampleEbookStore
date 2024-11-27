using Core.DTOs;
using Core.Entities;

namespace Core.Interfaces.Services.Assuntos;

public interface IAssuntoService
{
  Task<Assunto> GetAssuntoAsync(Guid id);
  Task<GenericResponse<Assunto>> CreateAssuntoAsync(Assunto entity);
  Task<GenericResponse<Assunto>> UpdateAssuntoAsync(int id, Assunto entity);
  Task<GenericResponse<bool>> ExcludeAssuntoAsync(int id);
}
