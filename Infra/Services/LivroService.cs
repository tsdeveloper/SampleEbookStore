using Core.DTOs;
using Core.Entities;
using Core.Interfaces.Services.Livros;

namespace Infra.Services;

public class LivroService : ILivroService
{
    public async Task<GenericResponse<Livro>> CreateLivroAsync(Livro entity)
    {
        throw new NotImplementedException();
    }

    public async Task<GenericResponse<bool>> ExcludeLivroAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<Livro> GetLivroAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<GenericResponse<Livro>> UpdateLivroAsync(Guid uid, Livro entity)
    {
        throw new NotImplementedException();
    }
}
