using AutoMapper;
using Core.DTOs.Livros;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Repositories.Livros;
using Core.Interfaces.Services.Assuntos;
using Core.Interfaces.Services.Autores;
using Core.Interfaces.Services.Livros;
using DinkToPdf.Contracts;
using FluentValidation;
using Infra.Data;
using Infra.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;

namespace API.Controllers.Test.Service;

public class BaseTestService : WebApplicationFactory<Program>
{

internal Dictionary<string, string> OverrideConfiguration = new();
  private readonly WebApplicationFactory<Program> _factory;

  protected Mock<ILivroRepository> _repoMockILivro;
  protected Mock<IGenericRepository<Livro>> _genericMockILivro;
  protected Mock<IAutorRepository> _reporMockIAutor;
  protected Mock<IAssuntoRepository> _reporMockIAssunto;
  protected Mock<IValidator<LivroCriarDto>> _validatorMockLivroCriarDto;
  protected Mock<IValidator<LivroAtualizarDto>> _validatorMockLivroAtualizarDto;
  protected Mock<IUnitOfWork> _repoMockIUnitOfWork;
  protected Mock<IMapper> _repoMockIMapper;
  protected Mock<IConverter> _repoMockIConverter;
  protected ILivroRepository _repoLivro => _repoMockILivro.Object;
  protected LivroService _serviceLivro;
  protected Mock<ILivroService> _serviceILivro;
  protected readonly HttpClient _httpClient;

  public BaseTestService()
  {
        _httpClient = CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
            BaseAddress = new Uri("http://localhost:7001/api")
        });
    LoadApplicationMockServices();
    LoadApplicationServices();
  }

  private void LoadApplicationMockServices()
  {
    _repoMockILivro = new Mock<ILivroRepository>();
    _genericMockILivro = new Mock<IGenericRepository<Livro>>();
    _validatorMockLivroCriarDto = new Mock<IValidator<LivroCriarDto>>();
    _validatorMockLivroAtualizarDto = new Mock<IValidator<LivroAtualizarDto>>();
    _repoMockIUnitOfWork = new Mock<IUnitOfWork>();
    _repoMockIMapper = new Mock<IMapper>();
    _repoMockIConverter = new Mock<IConverter>();
        _reporMockIAutor = new Mock<IAutorRepository>();
        _reporMockIAssunto = new Mock<IAssuntoRepository>();
        _serviceILivro = new Mock<ILivroService>();
  }

  private void LoadApplicationServices()
  {
    _serviceLivro = new LivroService(_repoMockIUnitOfWork.Object, _repoMockIMapper.Object, _repoMockILivro.Object);
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
        builder.ConfigureTestServices((services) =>
        {
            // Remove the existing DbContextOptions
            services.RemoveAll(typeof(DbContextOptions<SampleEbookStoreContext>));
            services.RemoveAll(typeof(IGenericRepository<>));
            services.RemoveAll<IUnitOfWork>();
            services.RemoveAll<ILivroRepository>();
            services.RemoveAll<ILivroService>();
            services.RemoveAll<IAutorService>();
            services.RemoveAll<IAssuntoService>();

            var serviceProvider = services.BuildServiceProvider();
            var scope = serviceProvider.CreateScope();
            // Register a new DBContext that will use our test connection string
            var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            string? connString = GetConnectionString(config);
            services.AddSqlServer<SampleEbookStoreContext>(connString);

            // Delete the database (if exists) to ensure we start clean
            SampleEbookStoreContext dbContext = CreateDbContext(services);
        });
    }
    private static string? GetConnectionString(IConfiguration config)
    {
            var connString = config.GetConnectionString("DEV-DOCKER-SQLSERVER");
        return connString;
    }

    private static SampleEbookStoreContext CreateDbContext(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<SampleEbookStoreContext>();
        return dbContext;
    }

}
