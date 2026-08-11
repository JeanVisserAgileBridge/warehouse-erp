using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;
using WarehouseERP.Infrastructure.DependencyInjection;

var builder = FunctionsApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Build().Run();
