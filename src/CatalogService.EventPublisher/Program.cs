using CatalogService.Infrastructure;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

var isTestingEnv = builder.Environment.IsEnvironment("Testing");
builder.Services.AddInfrastructure(builder.Configuration, isTestingEnv);

builder.Build().Run();
