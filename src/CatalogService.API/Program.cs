using CatalogService.API.Extensions;
using CatalogService.Application;
using CatalogService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var isTestingEnv = builder.Environment.IsEnvironment("Testing");

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration, isTestingEnv);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapEndpoints();

app.Run();

// Make the implicit Program class public so test projects can access it
public partial class Program
{ }
