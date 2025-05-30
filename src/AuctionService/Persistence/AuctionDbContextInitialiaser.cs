using AuctionService.Entities;
using Bogus;
using Contracts;

namespace AuctionService.Persistence;

public class AuctionDbContextInitialiaser(AuctionDbContext context, IPublishEndpoint publish)
{
    private readonly AuctionDbContext _context = context;
    private readonly IPublishEndpoint _publish = publish;

    public async Task InitialiseAsync(CancellationToken cancellationToken)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        try
        {
            if (_context.Database.GetMigrations().Any())
            {
                if ((await _context.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
                {
                    await _context.Database.MigrateAsync(cancellationToken);
                }
                if (await _context.Database.CanConnectAsync(cancellationToken))
                {
                    await SeedAsync(cancellationToken);
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        if (!await _context.Auctions.AnyAsync(cancellationToken))
        {
            var auctions = new Faker<Auction>()
                .RuleFor(a => a.Id, f => Guid.NewGuid())
                .RuleFor(a => a.Seller, f => f.Company.CompanyName())
                .RuleFor(a => a.Winner, f => f.Person.FullName)
                .RuleFor(a => a.SoldAmount, f => f.Random.Int(1, 100))
                .RuleFor(a => a.CurrentHighBid, f => f.Random.Int(1, 100))
                .RuleFor(a => a.AuctionEnd, f => f.Date.Future())
                .RuleFor(a => a.Status, f => f.PickRandom<AuctionStatus>())
                .RuleFor(a => a.Item, f => new Item
                {
                    Id = Guid.NewGuid(),
                    Make = f.Vehicle.Manufacturer(),
                    Model = f.Vehicle.Model(),
                    Year = f.Date.Past(10).Year,
                    Color = f.Commerce.Color(),
                    Mileage = f.Random.Int(1000, 200000),
                    ImageUrl = f.Image.PicsumUrl()
                })
                .Generate(10);

            await _context.Auctions.AddRangeAsync(auctions, cancellationToken);

            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (result)
            {
                foreach (var auction in auctions)
                {
                    await _publish.Publish(auction.Adapt<AuctionCreated>(), cancellationToken);
                }
            }
        }
    }
}
