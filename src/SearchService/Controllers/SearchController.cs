using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Controllers;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Item>>> SearchItems(string? searchTerm, int pageNumber = 1, int pageSize = 4)
    {
        var query = DB.PagedSearch<Item>();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query
                .Match(Search.Full, searchTerm)
                .SortByTextScore();
        }

        var queryResult = await query
            .Sort(s => s.Ascending(i => i.Name))
            .PageNumber(pageNumber)
            .PageSize(pageSize)
            .ExecuteAsync();

        return Ok(new
        {
            results = queryResult.Results,
            pageCount = queryResult.PageCount,
            totalCount = queryResult.TotalCount
        });
    }
}