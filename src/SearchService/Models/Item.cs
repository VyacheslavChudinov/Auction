using System.ComponentModel.DataAnnotations;
using MongoDB.Entities;

namespace SearchService.Models;

public class Item : Entity
{
    public required decimal ReservePrice { get; set; }
    public decimal CurrentHighBid { get; set; }
    public int SoldAmount { get; set; }
    [MaxLength(100)] public string? Seller { get; set; }
    [MaxLength(100)] public string? Winner { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public required DateTime AuctionEnd { get; set; }
    public required string Status { get; set; }

    [MaxLength(100)] public required string Name { get; set; }
    [MaxLength(10000)] public required string Description { get; set; }
    [MaxLength(1000)] public string? ImageUrl { get; set; }
}