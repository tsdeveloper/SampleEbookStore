using Core.Entities;
using Core.Interfaces.Repositories.Livros;
using Core.Specification.Livros;
using Moq;

namespace API.Controllers.Test.Mocks;

public static class MockLivroRepository
{
    public static Mock<ILivroRepository> MockGetByIdAsync(this Mock<ILivroRepository> mock, Livro @return)
    {
        mock.Setup(m => m.GetByIdAsync(It.IsAny<LivroObterTodosLivrosByFiltroSpecification>())).ReturnsAsync(@return);
        return mock;
    }
}
