using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using API.Errors;
using Core.Interfaces;
using Core.Interfaces.Repositories.Livros;
using Core.Interfaces.Services.Assuntos;
using Core.Interfaces.Services.Autores;
using Core.Interfaces.Services.Livros;
using DinkToPdf;
using DinkToPdf.Contracts;
using FluentValidation;
using Infra.Data;
using Infra.Repositories;
using Infra.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            //var architectureFolder = (IntPtr.Size == 8) ? "64 bit" : "32 bit";
            //var wkHtmlToPdfPath = Path.Combine(Environment.CurrentDirectory, $"wkhtmltox\\v0.12.4\\{architectureFolder}\\libwkhtmltox");
            //CustomAssemblyLoadContext context = new CustomAssemblyLoadContext();
            //context.LoadUnmanagedLibrary(wkHtmlToPdfPath);
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

            var factory = services.BuildServiceProvider().GetRequiredService<ILoggerFactory>();
            ILogger log = factory.CreateLogger("Start Application Extensions");

            var IsDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
            var connectionString = IsDevelopment
           ? configuration.GetConnectionString("DEV-DOCKER-SQLSERVER")
           : configuration.GetConnectionString("PRD-DOCKER-SQLSERVER");

           log.LogInformation("connectionString", connectionString);
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddDbContext<SampleEbookStoreContext>(options =>
                options.UseSqlServer(connectionString, m => m.MigrationsAssembly("Infra")));

            #region Repositories
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IAssuntoRepository, AssuntoRepository>();
            services.AddScoped<ILivroRepository, LivroRepository>();
            services.AddScoped<IAutorRepository, AutorRepository>();
            #endregion

            #region Services
            services.AddScoped<ILivroService, LivroService>();
            services.AddScoped<IAutorService, AutorService>();
            services.AddScoped<IAssuntoService, AssuntoService>();

            #endregion

            services.Configure<ApiBehaviorOptions>(o =>
                    {
                        o.InvalidModelStateResponseFactory = actionConext =>
                        {
                            var errors = actionConext.ModelState
                                .Where(e => e.Value.Errors.Count > 0)
                                .SelectMany(x => x.Value.Errors)
                                .Select(x => x.ErrorMessage).ToArray();
                            var errorResponse = new ApliValidationErrorResponse
                            {
                                Errors = errors
                            };

                            return new BadRequestObjectResult(errorResponse);
                        };
                    });

            services.AddValidatorsFromAssembly(Assembly.Load("Core"));

            return services;
        }

    }
}