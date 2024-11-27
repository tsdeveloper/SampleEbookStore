using Bogus;
using Core.Entities;
using Core.Interfaces;
using Infra.Data;
using Infra.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infra.DataAccess.Test;

[TestFixture]
public class LivroRepositoryTests
{
    private List<Livro> _fakerLivros;
   
    public LivroRepositoryTests()
    {
        var livroIds = 1;
        _fakerLivros = new Faker<Livro>()
        .CustomInstantiator(f => new Livro(livroIds++))
        .RuleFor(x => x.Titulo, f => f.Commerce.Department())
        .RuleFor(x => x.Editora, f => f.Commerce.ProductDescription())
        .RuleFor(x => x.Edicao, f => f.Random.Int(1,30))
        .RuleFor(x => x.AnoPublicacao, f => f.Random.Int(2010,2024).ToString())
        .Generate(10);
    }

    [Test]
    public void SalvarLivroEValidarValoresInseridosDatabase()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<SampleEbookStoreContext>()
                        .UseInMemoryDatabase(databaseName: "temp_ebookstore").Options;

        //act
        using (var context = new SampleEbookStoreContext(options))
        {
            foreach (var livro in _fakerLivros)
            {
                var unitOfWork = new UnitOfWork(context);
                unitOfWork.BeginTransactionAsync().ConfigureAwait(false);
                var repository = new LivroRepository(context);
                repository.Add(livro);
                unitOfWork.SaveChangesAsync().ConfigureAwait(false);
                unitOfWork.CommitAsync().ConfigureAwait(false);
            }

        }

        using (var context = new SampleEbookStoreContext(options))
        {
            var livro = context.Set<Livro>().FirstOrDefault(x => x.CodI == 1);
            Assert.AreEqual(_fakerLivros.FirstOrDefault().CodI, livro.CodI);
        }
    }
}