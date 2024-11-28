using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Repositories.Livros;
using Core.Interfaces.Services.Livros;
using Core.Specification.Livros;
using Core.Specification.Livros.SpecParams;

namespace Infra.Services;

public class LivroService : ILivroService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILivroRepository _repoLivro;
    public LivroService(IUnitOfWork unitOfWork, IMapper mapper, ILivroRepository repoLivro)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _repoLivro=repoLivro;
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
    public async Task<GenericResponse<Livro>> UpdateLivroAsync(int id, Livro entityUpdate)
    {
         var response = new GenericResponse<Livro>();

        var spec = new LivroObterTodosLivrosByFiltroSpecification(new LivroSpecParams { CodL = id, IncluirAssuntos = true, IncluirAutores = true });
        var entity = await _repoLivro.GetEntityWithSpec(spec);

        if (entity !=null)
        {
            if (entityUpdate.Livro_AutorList.Any())
            await AtualizarListaAutores(entity.Livro_AutorList, entityUpdate.Livro_AutorList);

            if (entityUpdate.Livro_AssuntoList.Any())
            await AtualizarListaAssuntos(entity.Livro_AssuntoList, entityUpdate.Livro_AssuntoList);

            await _unitOfWork.BeginTransactionAsync();
            _unitOfWork.Repository<Livro>().Update(entityUpdate);
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
        response.Error.Message = $"Não foi possível encontrar o livro {entityUpdate.Titulo}";
        return response;
    }

    private async Task<List<Livro_Assunto>> AtualizarListaAssuntos(ICollection<Livro_Assunto> livro_Assuntos, ICollection<Livro_Assunto> livro_AssuntoUpdate)
    {

        var novosLivrosAssuntos = livro_AssuntoUpdate.Where(x => !livro_Assuntos.Any(d => d.Livro_CodL == x.Livro_CodL && d.Assunto_CodAs == x.Assunto_CodAs));
        var obterLivroAssuntosExecluidos = livro_Assuntos.Where(x => !livro_AssuntoUpdate.Any(d => d.Livro_CodL == x.Livro_CodL && d.Assunto_CodAs == x.Assunto_CodAs));

        await _repoLivro.AdicionarLivro_AssuntoRelacionado(novosLivrosAssuntos.ToList());
        await _repoLivro.RemoverLivro_AssuntoRelacionado(obterLivroAssuntosExecluidos.ToList());

        return novosLivrosAssuntos.ToList();
    }

    private async  Task<List<Livro_Autor>> AtualizarListaAutores(ICollection<Livro_Autor> entityAutores, ICollection<Livro_Autor> entityAutoresUpdate)
    {
        var obterAutoresNovos = entityAutoresUpdate.Where(x => !entityAutores.Any(d => d.Livro_CodL == x.Livro_CodL && d.Autor_CodAu == x.Autor_CodAu));
        var obterAutoresExluidos = entityAutores.Where(x => !entityAutoresUpdate.Any(d => d.Livro_CodL == x.Livro_CodL && d.Autor_CodAu == x.Autor_CodAu))
                .Select(x => new Livro_Autor { Autor_CodAu = x.Autor_CodAu, Livro_CodL= x.Livro_CodL });
        await _repoLivro.AdicionarLivroAutorRelacionado(obterAutoresNovos.ToList());
        await _repoLivro.RemoverLivroAutorRelacionado(obterAutoresExluidos.ToList());

        return obterAutoresNovos.ToList();
    }

    public async Task<GenericResponse<bool>> ExcludeLivroAsync(int id)
    {
        var response = new GenericResponse<bool>();

        var spec = new LivroObterTodosLivrosByFiltroSpecification(new LivroSpecParams { CodL = id });
        var entity = await _repoLivro.GetEntityWithSpec(spec);

        if (entity !=null)
        {
            await _unitOfWork.BeginTransactionAsync();
            _unitOfWork.Repository<Livro>().Delete(entity);
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
        response.Error.Message = $"Não foi possível encontrar o livro {id}";
        return response;
    }

    public async Task<Livro> GetLivroAsync(Guid id)
    {
        throw new NotImplementedException();
    }

}
