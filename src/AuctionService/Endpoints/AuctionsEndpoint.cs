using AuctionService.DTOs;
using AuctionService.Persistence;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AuctionService.Endpoints;

public static class AuctionsEndpoint
{
    public static void MapAuctionsEndpoints(this IEndpointRouteBuilder builder)
    {
        var auctions = builder.MapGroup("api/auctions").WithTags("Auctions");

        auctions.MapGet("/", GetAuctionsAsync);

        auctions.MapGet("/{id:guid}", GetAuctionAsync);
    }

    private static async Task<Ok<List<AuctionDto>>> GetAuctionsAsync(AuctionDbContext context, CancellationToken cancellationToken)
    {
        var auctions = await context.Auctions
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return TypedResults.Ok(auctions.Adapt<List<AuctionDto>>());
    }

    private static async Task<Results<Ok<AuctionDto>, NotFound>> GetAuctionAsync(
        Guid id,
        AuctionDbContext context,
        CancellationToken cancellationToken)
    {
        var auction = await context.Auctions
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (auction is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(auction.Adapt<AuctionDto>());
    }
}
