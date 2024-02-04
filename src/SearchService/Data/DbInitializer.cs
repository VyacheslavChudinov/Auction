using System.Text.Json;
using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Data;

public static class DbInitializer
{
    public static async Task InitDb(WebApplication app)
    {
        await DB.InitAsync("SearchDB",
            MongoClientSettings.FromConnectionString(app.Configuration.GetConnectionString("MongoDbConnection")));

        await DB.Index<Item>()
            .Key(i => i.Name, KeyType.Text)
            .CreateAsync();

        var count = await DB.CountAsync<Item>();
        if (count == 0)
        {
            Console.WriteLine("Seeding data...");
            var auctionsData = await File.ReadAllTextAsync("Data/auctions.json");
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var auctions = JsonSerializer.Deserialize<List<Item>>(auctionsData, options);
            if (auctions is null)
            {
                Console.WriteLine("Seeding failed.");

                return;
            }

            await DB.SaveAsync(auctions);
        }
    }
}