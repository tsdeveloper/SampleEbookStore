using Castle.Core.Resource;
using Core.Entities;
using Core.Interfaces.Repositories.Livros;
using Dapper;
using Dapper.Contrib.Extensions;
using Infra.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Infra.Repositories;

public class LivroRepository : GenericRepository<Livro>, ILivroRepository
{
    private readonly SampleEbookStoreContext _context;

    public LivroRepository(SampleEbookStoreContext context) : base(context)
    {
        _context=context;
    }

    public async Task<bool> AdicionarLivroAutorRelacionado(List<Livro_Autor> novosLivroAutores)
    {
        var result = true;
        using (var context = new SqlConnection(_context.Database.GetConnectionString()))
        {
            foreach (var livroAutor in novosLivroAutores)
            {
                var sql = "INSERT INTO Livro_Autor (Livro_CodL, Autor_CodAu) VALUES(@Livro_CodL, @Autor_CodAu);";
                result = await context.ExecuteAsync(sql, livroAutor) > 0;

                if (!result)
                    return result;
            }

        }

        return result;
    }


    public async Task<bool> RemoverLivroAutorRelacionado(List<Livro_Autor> excluidoLivroAutores)
    {
        var result = true;
        using (var context = new SqlConnection(_context.Database.GetConnectionString()))
        {
            foreach (var livroAutor in excluidoLivroAutores)
            {
                var sql = @"
                            DELETE FROM Livro_Autor
                            WHERE Autor_CodAu=@Autor_CodAu AND Livro_CodL=@Livro_CodL";

                result = await context.ExecuteAsync(sql, livroAutor) > 0;

                if (!result)
                    return result;
            }

        }

        return result;
    }

    public async Task<bool> AdicionarLivro_AssuntoRelacionado(List<Livro_Assunto> livro_Assuntos)
    {
        var result = true;
        using (var context = new SqlConnection(_context.Database.GetConnectionString()))
        {
            foreach (var livroAssunto in livro_Assuntos)
            {
                var sql = @"INSERT INTO Livro_Assunto
                            (Livro_CodL, Assunto_CodAs)
                            VALUES(@Livro_CodL, @Assunto_CodAs)";
                result = await context.ExecuteAsync(sql, livroAssunto) > 0;

                if (!result)
                    return result;
            }

        }

        return result;
    }


    public async Task<bool> RemoverLivro_AssuntoRelacionado(List<Livro_Assunto> livro_Assuntos)
    {
        var result = true;
        using (var context = new SqlConnection(_context.Database.GetConnectionString()))
        {
            foreach (var livroAssunto in livro_Assuntos)
            {
                var sql = @"
                            DELETE FROM Livro_Assunto
                            WHERE Livro_CodL=@Livro_CodL AND Assunto_CodAs=@Assunto_CodAs";

                result = await context.ExecuteAsync(sql, livroAssunto) > 0;

                if (!result)
                    return result;
            }

        }

        return result;
    }

    public async Task<IReadOnlyList<LivroRelatorio>> GetTodosLivrosByView()
    {
        IEnumerable<LivroRelatorio> result;
        var context = new SqlConnection(_context.Database.GetConnectionString());
      
            var sql = "SELECT * FROM v_livros";
        result = await context.QueryAsync<LivroRelatorio>(sql);


        return result.ToList();

    }
}
