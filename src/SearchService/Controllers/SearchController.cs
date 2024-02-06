using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.RequestHelpers;

namespace SearchService.Controllers;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Item>>> SearchItems([FromQuery] SearchParams searchParams)
    {
        var query = DB.PagedSearch<Item, Item>();

        if (!string.IsNullOrEmpty(searchParams.SearchTerm)) query.Match(Search.Full, searchParams.SearchTerm);

        query = searchParams.OrderBy?.ToLower() switch
        {
            "name" => query.Sort(s => s.Ascending(i => i.Name)),
            "new" => query.Sort(s => s.Ascending(i => i.CreatedAt)),
            _ => query.Sort(s => s.Ascending(i => i.AuctionEnd))
        };

        query = searchParams.FilterBy?.ToLower() switch
        {
            "finished" => query.Match(i => i.AuctionEnd < DateTime.UtcNow),
            "endingsoon" => query.Match(i => i.AuctionEnd < DateTime.UtcNow.AddHours(8)),
            _ => query.Match(i => i.AuctionEnd > DateTime.UtcNow)
        };

        if (!string.IsNullOrEmpty(searchParams.Seller)) query.Match(i => i.Seller == searchParams.Seller);

        if (!string.IsNullOrEmpty(searchParams.Winner)) query.Match(i => i.Seller == searchParams.Winner);

        var queryResult = await query
            .PageNumber(searchParams.PageNumber)
            .PageSize(searchParams.PageSize)
            .ExecuteAsync();

        return Ok(new
        {
            results = queryResult.Results,
            pageCount = queryResult.PageCount,
            totalCount = queryResult.TotalCount
        });
    }
}