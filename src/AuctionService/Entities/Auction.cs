namespace AuctionService.Entities;

public class Auction
{
    public Guid Id { get; set; }
    public decimal ReservePrice { get; set; }
    public decimal CurrentHighBid { get; set; }
    public int? SoldAmount { get; set; }
    public string? Seller { get; set; }
    public string? Winner { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public required DateTime ActionEnd { get; set; }
    public required Status Status { get; set; }
    public required Item Item { get; set; }
}