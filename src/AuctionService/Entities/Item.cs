namespace AuctionService.Entities;

public class Item
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }

    public required Auction Auction { get; set; }
    public required Guid AuctionId { get; set; }
}