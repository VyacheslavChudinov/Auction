using System.ComponentModel.DataAnnotations;

namespace AuctionService.DTOs;

public class UpdateAuctionDto
{
    [MaxLength(100)] public required string Name { get; set; }
    [MaxLength(10000)] public required string Description { get; set; }
    [MaxLength(1000)] public string? ImageUrl { get; set; }
}