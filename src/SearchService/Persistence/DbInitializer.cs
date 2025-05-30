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

    }
}
