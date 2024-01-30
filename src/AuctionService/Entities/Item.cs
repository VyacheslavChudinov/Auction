using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionService.Entities;

[Table("Items")]
public class Item
{
    public required Guid Id { get; set; }
    [MaxLength(100)] public required string Name { get; set; }
    [MaxLength(10000)] public required string Description { get; set; }
    [MaxLength(1000)] public string? ImageUrl { get; set; }
    
    public Auction? Auction { get; set; }
    public Guid AuctionId { get; set; }
}