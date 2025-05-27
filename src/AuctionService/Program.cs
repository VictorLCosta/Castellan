using AuctionService.Endpoints;
using AuctionService.Persistence;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDbContext<AuctionDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<AuctionDbContextInitialiaser>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMassTransit(x =>
{
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("auction", false));

    x.AddEntityFrameworkOutbox<AuctionDbContext>(o =>
    {
        o.QueryDelay = TimeSpan.FromSeconds(10);
        o.UsePostgres();
        o.UseBusOutbox();
    });

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMq:Host"], "/", h => 
        {
            h.Username(builder.Configuration.GetValue("RabbitMq:Username", "user")); 
            h.Password(builder.Configuration.GetValue("RabbitMq:Password", "password"));
        });
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapAuctionsEndpoints();
    app.MapScalarApiReference("api-docs");

    CancellationTokenSource source = new();
    CancellationToken token = source.Token;

    using var scope = app.Services.CreateScope();
    await scope.ServiceProvider.GetRequiredService<AuctionDbContextInitialiaser>()
        .InitialiseAsync(token);
}

app.UseHttpsRedirection();

await app.RunAsync();