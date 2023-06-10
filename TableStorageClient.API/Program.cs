using Azure.Data.Tables;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Serilog;
using TableStorageClient.Infrastructure.Configure;
using TableStorageClient.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTableStorageClient(builder.Configuration);

builder.Host.UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .WriteTo.Console());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(swaggerUI => swaggerUI.DisplayRequestDuration());
}

app.MapControllers();

app.Run();