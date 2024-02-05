using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers;

[ApiController]
[Route("api/auctions")]
public class AuctionsController(AuctionDbContext context, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions(string? date)
    {
        var query = context.Auctions
            .OrderBy(a => a.Item.Name)
            .AsQueryable();

        if (!string.IsNullOrEmpty(date))
        {
            try
            {
                var parsedDate = DateTime.Parse(date).ToUniversalTime();
                query = query.Where(i => i.UpdatedAt.CompareTo(parsedDate) > 0);
            }
            catch (Exception e)
            {
                return BadRequest(new ProblemDetails { Title = e.Message });
            }
        }

        return await query
            .AsNoTracking()
            .ProjectTo<AuctionDto>(mapper.ConfigurationProvider)
            .ToListAsync();
    }


    [HttpGet("{id:guid}", Name = "GetAuctionById")]
    public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
    {
        var auction = await context.Auctions
            .AsNoTracking()
            .Include(a => a.Item)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (auction is null)
        {
            return NotFound();
        }

        return mapper.Map<AuctionDto>(auction);
    }

    [HttpPost]
    public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDTO createAuctionDto)
    {
        var auction = mapper.Map<Auction>(createAuctionDto);

        // TODO: replace to current user when identity is implemented
        auction.Seller = "User";

        await context.Auctions.AddAsync(auction);

        var hasChanges = await context.SaveChangesAsync() > 0;
        if (!hasChanges)
        {
            return BadRequest(new ProblemDetails { Title = "Problem creating new auction" });
        }

        return CreatedAtAction(nameof(GetAuctionById), new { auction.Id }, mapper.Map<AuctionDto>(auction));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDTO updateAuctionDto)
    {
        var auction = await context.Auctions
            .Include(a => a.Item)
            .FirstOrDefaultAsync(a => a.Id == id);
        if (auction is null) return NotFound();

        // TODO: check if current user is the seller

        mapper.Map(updateAuctionDto, auction);
        context.Auctions.Update(auction);

        var hasChanges = await context.SaveChangesAsync() > 0;
        if (!hasChanges) return BadRequest(new ProblemDetails { Title = "Problem updating auction" });

        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteAuction(Guid id)
    {
        var auction = await context.Auctions.FindAsync(id);
        if (auction is null) return NotFound();

        // TODO: check if current user is the seller

        context.Auctions.Remove(auction);
        var hasChanges = await context.SaveChangesAsync() > 0;
        if (!hasChanges) return BadRequest(new ProblemDetails { Title = "Problem deleting auction" });

        return Ok();
    }
}