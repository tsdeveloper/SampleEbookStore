using Bogus;
using Core.Entities;
using Infra.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Seed;

public class DbInitializer
{
    public static void InitDb(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        SeedData(scope.ServiceProvider.GetService<SampleEbookStoreContext>());
    }
    private static void SeedData(SampleEbookStoreContext context, bool generateMockSeeds = false)
    {
        context.Database.Migrate();

        if (generateMockSeeds)
        {
            SeedAutorFaker(context);
            SeedLivroFaker(context);
        }
    }

    private static void SeedAutorFaker(SampleEbookStoreContext context)
    {
        if (!context.Set<Autor>().Any())
        {
            var fakeCategory = new Faker<Autor>()
                .RuleFor(o => o.Nome, f => f.Commerce.Department())
                .Generate(25);

            context.AddRange(fakeCategory);
            context.SaveChanges();

        }
    }
    private static void SeedLivroFaker(SampleEbookStoreContext context)
    {
        if (!context.Set<Livro>().Any())
        {
            var fakeCategory = new Faker<Livro>()
                .RuleFor(o => o.Titulo, f => f.Commerce.Department())
                .RuleFor(o => o.Edicao, f => f.Random.Int(1, 50))
                .RuleFor(o => o.Editora, f => f.Commerce.ProductName())
                .RuleFor(o => o.AnoPublicacao, f => f.Random.Int(2010, 2023).ToString())
                .Generate(25);

            context.AddRange(fakeCategory);
            context.SaveChanges();

        }
    }
}
