using API.Controllers;
using API.Controllers.Test.Builder.Entities;
using API.Controllers.Test.Mocks;
using API.Controllers.Test.Service;
using Core.Specification.Livros;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Shouldly;
using static System.Net.Mime.MediaTypeNames;
using System.Net.Http.Json;
using Core.DTOs.Livros;
using API.Controllers.Test.Builder.DTOs;
using Core.Entities;
using Azure;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity.Core.Objects;

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
        _genericMockLivro.MockGetEntityWithSpec(new LivroBuilder().Default().Build());

        var request = new LivroBuilder().Default().Build();

        var resultMapperLivro = new LivroReturnDtoBuilder().Default().Build();

        _repoMockMapper.Setup(mapper => mapper.Map<LivroReturnDto>(It.IsAny<Livro>()))
.Returns(resultMapperLivro);

        var result = await _livroController.GetDetalhesPorId(request.CodI);

        var matchResponse = ((OkObjectResult)result.Result).Value as LivroReturnDto;

        matchResponse.ShouldNotBeNull();
        matchResponse.CodI.ShouldBeEquivalentTo(request.CodI);
        _genericMockLivro.Verify(x => x.GetEntityWithSpec(It.IsAny<LivroObterTodosLivrosByFiltroSpecification>()), Times.Once);
    }
}
