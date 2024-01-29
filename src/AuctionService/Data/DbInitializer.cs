using AuctionService.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Data;

public static class DbInitializer
{
    public static void Initialize(WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        SeedData(scope.ServiceProvider.GetService<AuctionDbContext>());
    }

    private static void SeedData(AuctionDbContext? context)
    {
        if (context is null) return;

        context.Database.Migrate();

        if (context.Auctions.Any()) return;

        var auctions = new List<Auction>
        {
            new()
            {
                Id = Guid.Parse("afbee524-5972-4075-8800-7d1f9d7b0a0c"),
                Status = Status.Finished,
                ReservePrice = 40000,
                Seller = "Andrew",
                AuctionEnd = DateTime.UtcNow.AddDays(3),
                Item = new Item
                {
                    Id = Guid.Parse("afbee345-1290-4075-8800-7d1f9d7b0a0c"),
                    Name = "Retro set",
                    Description = "11 different items in 80's style that'll make you look like a movie star.",
                    ImageUrl = "https://cdn.pixabay.com/photo/2017/04/06/11/24/fashion-2208045_1280.jpg"
                }
            },
            new()
            {
                Id = Guid.Parse("afbee123-5972-4075-8800-7d1f9d7b0a0c"),
                Status = Status.ReserveNotMet,
                ReservePrice = 30000,
                Seller = "Andrew",
                AuctionEnd = DateTime.UtcNow.AddDays(4),
                Item = new Item
                {
                    Id = Guid.Parse("afbee345-1234-4075-8800-7d1f9d7b0a0c"),
                    Name = "Retro set 2",
                    Description =
                        @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla porta eu justo at consequat. 
                    Maecenas bibendum, eros eget posuere imperdiet, nibh libero euismod nibh, sed accumsan nisl arcu quis diam.
                    Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae.",
                    ImageUrl = "https://cdn.pixabay.com/photo/2016/01/19/17/34/cameras-1149767_1280.jpg"
                }
            },
            new()
            {
                Id = Guid.Parse("afbee321-5972-4075-8800-7d1f9d7b0a0c"),
                Status = Status.Live,
                ReservePrice = 20000,
                Seller = "Andrew",
                AuctionEnd = DateTime.UtcNow.AddDays(5),
                Item = new Item
                {
                    Id = Guid.Parse("afbee345-1235-4075-8800-7d1f9d7b0a0c"),
                    Name = "Retro set 3",
                    Description =
                        @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla porta eu justo at consequat. 
                    Maecenas bibendum, eros eget posuere imperdiet, nibh libero euismod nibh, sed accumsan nisl arcu quis diam.
                    Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae.",
                    ImageUrl = "https://cdn.pixabay.com/photo/2015/07/20/00/57/vintage-car-852239_1280.jpg"
                }
            },
            new()
            {
                Id = Guid.Parse("afbee234-5972-4075-8800-7d1f9d7b0a0c"),
                Status = Status.Live,
                ReservePrice = 10000,
                Seller = "Andrew",
                AuctionEnd = DateTime.UtcNow.AddDays(6),
                Item = new Item
                {
                    Id = Guid.Parse("afbee345-1236-4075-8800-7d1f9d7b0a0c"),
                    Name = "Retro set 4",
                    Description =
                        @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla porta eu justo at consequat. 
                    Maecenas bibendum, eros eget posuere imperdiet, nibh libero euismod nibh, sed accumsan nisl arcu quis diam.
                    Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae.",
                    ImageUrl = "https://cdn.pixabay.com/photo/2018/06/17/13/59/cameras-3480569_1280.jpg"
                }
            },
            new()
            {
                Id = Guid.Parse("afbee345-5972-4075-8800-7d1f9d7b0a0c"),
                Status = Status.Live,
                ReservePrice = 50000,
                Seller = "Andrew",
                AuctionEnd = DateTime.UtcNow.AddDays(7),
                Item = new Item
                {
                    Id = Guid.Parse("afbee345-1237-4075-8800-7d1f9d7b0a0c"),
                    Name = "Retro set 5",
                    Description =
                        @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla porta eu justo at consequat. 
                    Maecenas bibendum, eros eget posuere imperdiet, nibh libero euismod nibh, sed accumsan nisl arcu quis diam.
                    Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae.",
                    ImageUrl = "https://cdn.pixabay.com/photo/2019/04/04/18/50/cassette-4103530_640.jpg"
                }
            }
        };

        context.AddRange(auctions);
        context.SaveChanges();
    }
}