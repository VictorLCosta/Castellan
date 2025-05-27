using AuctionService.DTOs;
using AuctionService.Entities;
using AuctionService.Persistence;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AuctionService.Endpoints;

public static class AuctionsEndpoint
{
    public static void MapAuctionsEndpoints(this IEndpointRouteBuilder builder)
    {
        var auctions = builder.MapGroup("api/auctions").WithTags("Auctions");

        auctions.MapGet("/", GetAuctionsAsync);

        auctions.MapGet("/{id:guid}", GetAuctionAsync);

        auctions.MapPost("/", CreateAuctionAsync);

        auctions.MapPut("/", UpdateAuctionAsync);

        auctions.MapDelete("/{id:guid}", DeleteAuctionAsync);
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

    private static async Task<Results<Created<AuctionDto>, BadRequest<string>>> CreateAuctionAsync(
        CreateAuctionDto auctionDto,
        AuctionDbContext context,
        IPublishEndpoint publishEndpoint,
        CancellationToken cancellationToken)
    {
        var auction = auctionDto.Adapt<Auction>();

        context.Auctions.Add(auction);
        var result = await context.SaveChangesAsync(cancellationToken) > 0;

        if (!result)
        {
            return TypedResults.BadRequest("Failed to create auction");
        }

        await publishEndpoint.Publish(auction.Adapt<AuctionCreated>(), cancellationToken);

        return TypedResults.Created($"/api/auctions/{auction.Id}", auction.Adapt<AuctionDto>());
    }

    private static async Task<Results<Ok<AuctionDto>, NotFound, BadRequest<string>>> UpdateAuctionAsync(
        UpdateAuctionDto auctionDto,
        AuctionDbContext context,
        CancellationToken cancellationToken)
    {
        var auction = await context.Auctions
            .FirstOrDefaultAsync(x => x.Id == auctionDto.Id, cancellationToken);

        if (auction is null)
        {
            return TypedResults.NotFound();
        }

        auctionDto.Adapt(auction);

        var result = await context.SaveChangesAsync(cancellationToken) > 0;

        if (!result)
        {
            return TypedResults.BadRequest("Failed to update auction");
        }

        return TypedResults.Ok(auction.Adapt<AuctionDto>());
    }

    private static async Task<Results<Ok<AuctionDto>, NotFound, BadRequest<string>>> DeleteAuctionAsync(
        Guid id,
        AuctionDbContext context,
        CancellationToken cancellationToken)
    {
        var auction = await context.Auctions
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (auction is null)
        {
            return TypedResults.NotFound();
        }

        context.Auctions.Remove(auction);
        var result = await context.SaveChangesAsync(cancellationToken) > 0;

        if (!result)
        {
            return TypedResults.BadRequest("Failed to delete auction");
        }

        return TypedResults.Ok(auction.Adapt<AuctionDto>());
    }
}
