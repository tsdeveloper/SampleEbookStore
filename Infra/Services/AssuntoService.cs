using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Services.Assuntos;
using Core.Specification.Assuntos;
using Core.Specification.Assuntos.SpecParams;
using static Dapper.SqlMapper;

namespace Infra.Services;

public class AssuntoService : IAssuntoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public AssuntoService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<GenericResponse<Assunto>> CreateAssuntoAsync(Assunto entity)
    {
        var response = new GenericResponse<Assunto>();

        await _unitOfWork.BeginTransactionAsync();

        _unitOfWork.Repository<Assunto>().Add(entity);

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
    public async Task<GenericResponse<Assunto>> UpdateAssuntoAsync(int id, Assunto entity)
    {
         var response = new GenericResponse<Assunto>();

        var spec = new AssuntoObterTodosAssuntosByFiltroSpecification(new AssuntoSpecParams { CodAs = id });
        var AssuntoExist = await _unitOfWork.Repository<Assunto>().GetExistEntityWithSpec(spec);

        if (AssuntoExist)
        {
            await _unitOfWork.BeginTransactionAsync();
            _unitOfWork.Repository<Assunto>().Update(entity);
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
        response.Error.Message = $"Não foi possível encontrar o Assunto {entity.Descricao}";
        return response;
    }

    public async Task<GenericResponse<bool>> ExcludeAssuntoAsync(int id)
    {
        var response = new GenericResponse<bool>();

        var spec = new AssuntoObterTodosAssuntosByFiltroSpecification(new AssuntoSpecParams { CodAs = id });
        var entity = await _unitOfWork.Repository<Assunto>().GetEntityWithSpec(spec);

        if (entity != null)
        {
            await _unitOfWork.BeginTransactionAsync();
            _unitOfWork.Repository<Assunto>().Delete(entity);
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
        response.Error.Message = $"Não foi possível encontrar o Assunto ID {id}";
        return response;
    }

    public async Task<Assunto> GetAssuntoAsync(Guid id)
    {
        throw new NotImplementedException();
    }

}
