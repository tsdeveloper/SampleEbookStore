using Castle.Core.Resource;
using Core.Entities;
using Core.Interfaces.Repositories.Livros;
using Core.Specification.Autors;
using Core.Specification.Autors.SpecParams;
using Dapper;
using Dapper.Contrib.Extensions;
using Infra.Data;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Infra.Repositories;

public class AutorRepository : GenericRepository<Autor>, IAutorRepository
{
    private readonly SampleEbookStoreContext _context;

    public AutorRepository(SampleEbookStoreContext context) : base(context)
    {
        _context=context;
    }

    public async Task<List<Autor>> GetListAllAutoresByIds(List<Livro_Autor> autores)
    {
        var result = new List<Autor>();

        foreach (var autoresItem in autores)
        {
            result.Add(await GetEntityWithSpec(new AutorObterTodosAutoresByFiltroSpecification(new AutorSpecParams { CodAu = autoresItem.Autor_CodAu})));
        }
        return result;
    }
}
