using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Services.Livros;
using Core.Specification.Livros;
using Core.Specification.Livros.SpecParams;

namespace Infra.Services;

public class LivroService : ILivroService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public LivroService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<GenericResponse<Livro>> CreateLivroAsync(Livro entity)
    {
        var response = new GenericResponse<Livro>();

        await _unitOfWork.BeginTransactionAsync();

        _unitOfWork.Repository<Livro>().Add(entity);

        var result = await _unitOfWork.SaveChangesAsync();

        if (result.Error != null)
        {
            await _unitOfWork.RollbackAsync();

            response.Error = result.Error;
            return response;
        }

        await _unitOfWork.CommitAsync();

        return response;
    }
    public async Task<GenericResponse<Livro>> UpdateLivroAsync(int id, Livro entity)
    {
         var response = new GenericResponse<Livro>();

        var spec = new LivroObterTodosLivrosByFiltroSpecification(new LivroSpecParams { CodI = id });
        var livroExist = await _unitOfWork.Repository<Livro>().GetExistEntityWithSpec(spec);

        if (livroExist)
        {
            await _unitOfWork.BeginTransactionAsync();
            _unitOfWork.Repository<Livro>().Update(entity);
            var result = await _unitOfWork.SaveChangesAsync();

            if (result.Error != null)
            {
                await _unitOfWork.RollbackAsync();

                response.Error = result.Error;
                return response;
            }

            await _unitOfWork.CommitAsync();

            return response;
        }

        response.Error = new MessageResponse();
        response.Error.Message = $"Não foi possível encontrar o livro {entity.Titulo}";
        return response;
    }

    public async Task<GenericResponse<bool>> ExcludeLivroAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Livro> GetLivroAsync(Guid id)
    {
        throw new NotImplementedException();
    }

}
