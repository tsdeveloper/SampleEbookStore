using Castle.Core.Resource;
using Core.Entities;
using Core.Interfaces.Repositories.Livros;
using Core.Specification.Assuntos;
using Core.Specification.Assuntos.SpecParams;
using Core.Specification.Autors;
using Core.Specification.Autors.SpecParams;
using Dapper;
using Dapper.Contrib.Extensions;
using Infra.Data;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Infra.Repositories;

public class AssuntoRepository : GenericRepository<Assunto>, IAssuntoRepository
{
    private readonly SampleEbookStoreContext _context;

    public AssuntoRepository(SampleEbookStoreContext context) : base(context)
    {
        _context=context;
    }

    public async Task<List<Assunto>> GetListAllAssuntosByIds(List<Livro_Assunto> assuntos)
    {
        var result = new List<Assunto>();

        foreach (var autoresItem in assuntos)
        {
            result.Add(await GetEntityWithSpec(new AssuntoObterTodosAssuntosByFiltroSpecification(new AssuntoSpecParams { CodAs = autoresItem.Assunto_CodAs})));
        }
        return result;
    }
}
