using AutoMapper;
using Core.DTOs.Livros;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Repositories.Livros;
using FluentValidation;
using Infra.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;

namespace API.Controllers.Test.Service;

public class BaseTestService : WebApplicationFactory<Program>
{

internal Dictionary<string, string> OverrideConfiguration = new();
  private readonly WebApplicationFactory<Program> _factory;

  protected Mock<ILivroRepository> _repoMockLivro;
  protected Mock<IGenericRepository<Livro>> _genericMockLivro;
  protected Mock<IValidator<LivroCriarDto>> _validatorMockLivroCriarDto;
  protected Mock<IValidator<LivroAtualizarDto>> _validatorMockLivroAtualizarDto;
  protected Mock<IUnitOfWork> _repoMockUnitOfWork;
  protected Mock<IMapper> _repoMockMapper;
  protected ILivroRepository _repoLivro => _repoMockLivro.Object;
  protected LivroService _serviceLivro;
  protected readonly HttpClient _httpClient;

  public BaseTestService()
  {
    _httpClient = CreateClient();
    LoadApplicationMockServices();
    LoadApplicationServices();
  }

  private void LoadApplicationMockServices()
  {
    _repoMockLivro = new Mock<ILivroRepository>();
    _genericMockLivro = new Mock<IGenericRepository<Livro>>();
    _validatorMockLivroCriarDto = new Mock<IValidator<LivroCriarDto>>();
    _validatorMockLivroAtualizarDto = new Mock<IValidator<LivroAtualizarDto>>();
    _repoMockUnitOfWork = new Mock<IUnitOfWork>();
    _repoMockMapper = new Mock<IMapper>();
  }

  private void LoadApplicationServices()
  {
    _serviceLivro = new LivroService(_repoMockUnitOfWork.Object, _repoMockMapper.Object);
  }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.ConfigureAppConfiguration((ctx, builder) =>
        {
          System.Console.WriteLine("configure host");
          var testDir = Path.GetDirectoryName(GetType().Assembly.Location);
          var configLocation = Path.Combine(testDir!, "testsettings.json");

          builder.Sources.Clear();
          builder.AddJsonFile(configLocation);
          builder.AddInMemoryCollection(OverrideConfiguration);

        });
    }
 
}
