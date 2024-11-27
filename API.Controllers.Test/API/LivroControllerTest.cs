using API.Controllers;
using API.Controllers.Test.Builder.Entities;
using API.Controllers.Test.Mocks;
using API.Controllers.Test.Service;
using Core.Specification.Livros;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Shouldly;

namespace API.Controllers.Test.API;

public class LivroControllerTest : BaseTestService
{
    private LivroController _livroController;
    private readonly BaseTestService _baseTestService;

    public LivroControllerTest()
    {
        _livroController = new LivroController(_genericMockLivro.Object,
        _serviceLivro,
        _repoMockMapper.Object,
        _validatorMockLivroCriarDto.Object,
        _validatorMockLivroAtualizarDto.Object);
    }

    [Fact]
    public async Task Step_01_LivroEncontrado_GetController()
    {
        _repoMockLivro.MockGetByIdAsync(new LivroBuilder().Default().Build());

        var request = new LivroBuilder().Default().Build();

        var result = await _livroController.GetDetalhesPorId(request.CodI);
        result.ShouldNotBeNull();
        result.Value.CodI.ShouldBeEquivalentTo(request.CodI);
        _repoMockLivro.Verify(x => x.GetByIdAsync(It.IsAny<LivroObterTodosLivrosByFiltroSpecification>()), Times.Once);
    }
}
