using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

public class AuctionCreatedConsumer(IMapper mapper) : IConsumer<AuctionCreated>
{
    public async Task Consume(ConsumeContext<AuctionCreated> context)
    {
        Console.WriteLine($"Consuming AuctionCreated event: Id => {context.Message.Id}");
        var item = mapper.Map<Item>(context.Message);

        if (item.Name.Any(c => !(char.IsLetterOrDigit(c) || char.IsWhiteSpace(c))))
        {
            throw new ArgumentException("Item name can only contain letters or digits.");
        }

        await item.SaveAsync();
    }
}