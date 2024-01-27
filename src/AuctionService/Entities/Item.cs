using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionService.Entities;

[Table("Items")]
public class Item
{
    public Guid Id { get; set; }
    [MaxLength(100)]
    public required string Name { get; set; }
    [MaxLength(10000)]
    public string? Description { get; set; }
    [MaxLength(1000)]
    public string? ImageUrl { get; set; }

    public required Auction Auction { get; set; }
    public required Guid AuctionId { get; set; }
}