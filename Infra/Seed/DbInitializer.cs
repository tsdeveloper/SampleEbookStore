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
    private static void SeedData(SampleEbookStoreContext context)
  {
    context.Database.Migrate();
    SeedAutorFaker(context);
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
}
