using AuctionService.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDbContext<AuctionDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<AuctionDbContextInitialiaser>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    CancellationTokenSource source = new();
    CancellationToken token = source.Token;

    using var scope = app.Services.CreateScope();
    await scope.ServiceProvider.GetRequiredService<AuctionDbContextInitialiaser>()
        .InitialiseAsync(token);
}

app.UseHttpsRedirection();

await app.RunAsync();