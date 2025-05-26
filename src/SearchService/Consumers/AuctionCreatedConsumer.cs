using Contracts;
using MassTransit;
using Mapster;
using SearchService.Entities;

namespace SearchService.Consumers;

public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
{
    public async Task Consume(ConsumeContext<AuctionCreated> context)
    {
        Console.WriteLine($"AuctionCreatedConsumer: {context.Message.Id}");

        var item = context.Message.Adapt<Item>();

        await item.SaveAsync();
    }
}
