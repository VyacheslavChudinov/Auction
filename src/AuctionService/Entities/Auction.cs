using System.ComponentModel.DataAnnotations;

namespace AuctionService.Entities;

public class Auction
{
    public Guid Id { get; set; }
    public decimal ReservePrice { get; set; }
    public decimal CurrentHighBid { get; set; }
    public int? SoldAmount { get; set; }
    [MaxLength(100)]
    public string? Seller { get; set; }
    [MaxLength(100)]
    public string? Winner { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public required DateTime ActionEnd { get; set; }
    public required Status Status { get; set; }
    public required Item Item { get; set; }
}