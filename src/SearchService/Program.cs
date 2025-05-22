using Scalar.AspNetCore;
using SearchService.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("api-docs");
}

await app.InitialiseDbAsync();

app.UseHttpsRedirection();

await app.RunAsync();
