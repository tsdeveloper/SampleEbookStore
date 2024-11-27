using API.Extensions;
using Infra.Data;
using Infra.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigins", policy =>
    {
        policy.AllowAnyHeader()
            .AllowAnyOrigin()
            .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddApplicationServices(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
try
{
    DbInitializer.InitDb(app);
}
catch (Exception ex)
{
    
    Console.WriteLine(ex);
}
app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();

app.UseCors("AllowOrigins");

app.Run();

