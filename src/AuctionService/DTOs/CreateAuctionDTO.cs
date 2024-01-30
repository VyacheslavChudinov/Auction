using System.ComponentModel.DataAnnotations;

namespace AuctionService.DTOs;

public class CreateAuctionDTO
{
    public required decimal ReservePrice { get; set; }
    public required DateTime AuctionEnd { get; set; }

    [MaxLength(100)] public required string Name { get; set; }
    [MaxLength(10000)] public required string Description { get; set; }
    [MaxLength(1000)] public required string ImageUrl { get; set; }
}