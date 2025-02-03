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

        _livroController = new LivroController(_genericMockILivro.Object, _serviceILivro.Object,
            _repoMockIMapper.Object, _validatorMockLivroCriarDto.Object, _validatorMockLivroAtualizarDto.Object,
            _repoMockIConverter.Object, _reporMockIAutor.Object, _reporMockIAssunto.Object,
            _repoLivro);
    }

    [Fact]
    public async Task Step_01_LivroEncontrado_GetController()
    {
        _genericMockILivro.MockGetEntityWithSpec(new LivroBuilder().Default().Build());

        var request = new LivroBuilder().Default().Build();

        var resultMapperLivro = new LivroReturnDtoBuilder().Default().Build();

        _repoMockIMapper.Setup(mapper => mapper.Map<LivroReturnDto>(It.IsAny<Livro>()))
.Returns(resultMapperLivro);

        var result = await _livroController.GetDetalhesPorId(request.CodL);

        var matchResponse = ((OkObjectResult)result.Result).Value as LivroReturnDto;

        matchResponse.ShouldNotBeNull();
        matchResponse.CodL.ShouldBeEquivalentTo(request.CodL);
        _genericMockILivro.Verify(x => x.GetEntityWithSpec(It.IsAny<LivroObterTodosLivrosByFiltroSpecification>()), Times.Once);
    }
}
