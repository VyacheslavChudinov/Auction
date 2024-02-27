using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers;

[ApiController]
[Route("api/auctions")]
public class AuctionsController(AuctionDbContext context, IMapper mapper, IPublishEndpoint publishEndpoint)
    : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions(string? date)
    {
        var query = context.Auctions
            .OrderBy(a => a.Item.Name)
            .AsQueryable();

        if (!string.IsNullOrEmpty(date))
            try
            {
                var parsedDate = DateTime.Parse(date).ToUniversalTime();
                query = query.Where(i => i.UpdatedAt.CompareTo(parsedDate) > 0);
            }
            catch (Exception e)
            {
                return BadRequest(new ProblemDetails { Title = e.Message });
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

        if (auction is null) return NotFound();

        return mapper.Map<AuctionDto>(auction);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto createAuctionDto)
    {
        var auction = mapper.Map<Auction>(createAuctionDto);
        if (User?.Identity?.Name is null) { return Forbid(); }

        auction.Seller = User.Identity.Name;

        await context.Auctions.AddAsync(auction);

        var createdAuction = mapper.Map<AuctionCreated>(auction);
        await publishEndpoint.Publish(createdAuction);

        var hasChanges = await context.SaveChangesAsync() > 0;
        if (!hasChanges) return BadRequest(new ProblemDetails { Title = "Problem creating new auction" });

        return CreatedAtAction(nameof(GetAuctionById), new { auction.Id }, createdAuction);
    }

    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDto updateAuctionDto)
    {
        var auction = await context.Auctions
            .Include(a => a.Item)
            .FirstOrDefaultAsync(a => a.Id == id);
        if (auction is null) return NotFound();

        if (User?.Identity?.Name is null || auction.Seller != User.Identity.Name) { return Forbid(); };

        mapper.Map(updateAuctionDto, auction);
        context.Auctions.Update(auction);

        var updatedAuction = mapper.Map<AuctionUpdated>(auction);
        await publishEndpoint.Publish(updatedAuction);

        var hasChanges = await context.SaveChangesAsync() > 0;
        if (!hasChanges) return BadRequest(new ProblemDetails { Title = "Problem updating auction" });

        return Ok();
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteAuction(Guid id)
    {
        var auction = await context.Auctions.FindAsync(id);
        if (auction is null) return NotFound();

        if (User?.Identity?.Name is null || auction.Seller != User.Identity.Name) { return Forbid(); };

        context.Auctions.Remove(auction);

        var deletedAuction = mapper.Map<AuctionDto>(auction);
        await publishEndpoint.Publish<AuctionDeleted>(deletedAuction);

        var hasChanges = await context.SaveChangesAsync() > 0;
        if (!hasChanges) return BadRequest(new ProblemDetails { Title = "Problem deleting auction" });

        return Ok();
    }
}