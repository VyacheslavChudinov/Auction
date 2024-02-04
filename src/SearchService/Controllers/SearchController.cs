using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Controllers;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Item>>> SearchItems(string? searchTerm)
    {
        var query = DB.Find<Item>();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query
                .Match(Search.Full, searchTerm)
                .SortByTextScore();
        }

        return await query
            .Sort(s => s.Ascending(i => i.Name))
            .ExecuteAsync();
    }
}