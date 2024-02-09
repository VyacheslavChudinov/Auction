using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

public class AuctionUpdatedConsumer(IMapper mapper) : IConsumer<AuctionUpdated>
{
    public async Task Consume(ConsumeContext<AuctionUpdated> context)
    {
        Console.WriteLine($"Consuming AuctionUpdated event: Id => {context.Message.Id}");
        var item = mapper.Map<Item>(context.Message);

        await DB.Update<Item>()
            .MatchID(context.Message.Id)
            .ModifyWith(item)
            .ExecuteAsync();
    }
}