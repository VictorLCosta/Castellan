using Bogus;
using MongoDB.Driver;
using SearchService.Entities;

namespace SearchService.Persistence;

public static class DbInitializer
{
    public static async Task InitialiseDbAsync(this WebApplication app)
    {
        await DB.InitAsync("SearchDb", MongoClientSettings
            .FromConnectionString(app.Configuration.GetConnectionString("MongoDb")));

        await DB.Index<Item>()
            .Key(i => i.Make, KeyType.Text)
            .Key(i => i.Model, KeyType.Text)
            .Key(i => i.Color, KeyType.Text)
            .CreateAsync();

        var count = await DB.CountAsync<Item>();

        if (count == 0)
        {
            var items = new Faker<Item>()
                .RuleFor(i => i.ReservedPrice, f => f.Random.Int(1000, 10000))
                .RuleFor(i => i.Seller, f => f.Person.FullName)
                .RuleFor(i => i.Winner, f => f.Person.FullName)
                .RuleFor(i => i.SoldAmount, f => f.Random.Int(0, 10))
                .RuleFor(i => i.CurrentHighBid, f => f.Random.Int(1000, 10000))
                .RuleFor(i => i.CreatedAt, f => f.Date.Past(10))
                .RuleFor(i => i.UpdatedAt, f => f.Date.Past(5))
                .RuleFor(i => i.AuctionEnd, f => f.Date.Future(10))
                .RuleFor(i => i.Status, f => f.PickRandom(new[] { "Active", "Completed", "Cancelled" }))
                .RuleFor(i => i.Make, f => f.Vehicle.Manufacturer())
                .RuleFor(i => i.Model, f => f.Vehicle.Model())
                .RuleFor(i => i.Color, f => f.Commerce.Color())
                .RuleFor(i => i.Year, f => f.Date.Past(10).Year)
                .RuleFor(i => i.Mileage, f => f.Random.Int(0, 200000))
                .RuleFor(i => i.ImageUrl, f => f.Image.PicsumUrl())
                .Generate(20);

            await DB.InsertAsync(items);
        }
    }
}
