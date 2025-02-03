using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Services.Autores;
using Core.Specification.Autors;
using Core.Specification.Autors.SpecParams;
using static Dapper.SqlMapper;

namespace Infra.Services;

public class AutorService : IAutorService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public AutorService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<GenericResponse<Autor>> CreateAutorAsync(Autor entity)
    {
        var response = new GenericResponse<Autor>();

        await _unitOfWork.BeginTransactionAsync();

        _unitOfWork.Repository<Autor>().Add(entity);

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
    public async Task<GenericResponse<Autor>> UpdateAutorAsync(int id, Autor entity)
    {
         var response = new GenericResponse<Autor>();

        var spec = new AutorObterTodosAutoresByFiltroSpecification(new AutorSpecParams { CodAu = id });
        var AutorExist = await _unitOfWork.Repository<Autor>().GetExistEntityWithSpec(spec);

        if (AutorExist)
        {
            await _unitOfWork.BeginTransactionAsync();
            _unitOfWork.Repository<Autor>().Update(entity);
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
        response.Error.Message = $"Não foi possível encontrar o Autor {entity.Nome}";
        return response;
    }

    public async Task<GenericResponse<bool>> ExcludeAutorAsync(int id)
    {
        var response = new GenericResponse<bool>();

        var spec = new AutorObterTodosAutoresByFiltroSpecification(new AutorSpecParams { CodAu = id });
        var entity = await _unitOfWork.Repository<Autor>().GetEntityWithSpec(spec);

        if (entity !=null)
        {
            await _unitOfWork.BeginTransactionAsync();
            _unitOfWork.Repository<Autor>().Delete(entity);
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
        response.Error.Message = $"Não foi possível encontrar o Autor {entity.Nome}";
        return response;
    }

    public async Task<Autor> GetAutorAsync(Guid id)
    {
        throw new NotImplementedException();
    }

}
