using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Repositories.Livros;
using Core.Specification.Livros;
using Moq;

namespace API.Controllers.Test.Mocks;

public static class MockLivroRepository
{
    public static Mock<IGenericRepository<Livro>> MockGetEntityWithSpec(this Mock<IGenericRepository<Livro>> mock, Livro @return)
    {
        mock.Setup(m => m.GetEntityWithSpec(It.IsAny<LivroObterTodosLivrosByFiltroSpecification>())).ReturnsAsync(@return);
        return mock;
    }
}
