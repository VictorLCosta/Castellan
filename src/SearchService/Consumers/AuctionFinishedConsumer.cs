using Contracts;
using SearchService.Entities;

namespace SearchService.Consumers;

public class AuctionFinishedConsumer : IConsumer<AuctionFinished>
{
    public async Task Consume(ConsumeContext<AuctionFinished> context)
    {
        var auction = await DB.Find<Item>().OneAsync(context.Message.AuctionId);

        if (auction == null) return;

        if (context.Message.ItemSold)
        {
            auction.Winner = context.Message.Winner;
            auction.SoldAmount = context.Message.Amount ?? 0;
        }

        auction.Status = "Finished";
        await auction.SaveAsync();
    }
}
