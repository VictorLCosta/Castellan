using Scalar.AspNetCore;
using SearchService.Endpoints;
using SearchService.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.MapSearchEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("api-docs");
}

await app.InitialiseDbAsync();

app.UseHttpsRedirection();

await app.RunAsync();
