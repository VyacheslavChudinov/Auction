using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

public class AuctionDeletedConsumer : IConsumer<AuctionDeleted>
{
    public async Task Consume(ConsumeContext<AuctionDeleted> context)
    {
        Console.WriteLine($"Consuming AuctionDeleted event: Id => {context.Message.Id}");

        await DB.DeleteAsync<Item>(context.Message.Id);
    }
}