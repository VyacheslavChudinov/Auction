using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Services;

public class AuctionServiceHttpClient(HttpClient httpClient, IConfiguration config)
{
    public async Task<List<Item>> GetItemsForSearchDb()
    {
        var lastUpdated = await DB
            .Find<Item, string>()
            .Sort(s => s.Descending(i => i.UpdatedAt))
            .Project(i => i.UpdatedAt.ToString())
            .ExecuteFirstAsync();

        var auctionsUrl = $"{config["AuctionServiceUrl"]}/api/auctions?date={lastUpdated}";
        return await httpClient.GetFromJsonAsync<List<Item>>(auctionsUrl) ?? [];
    }
}