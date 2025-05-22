using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SearchService.Entities;
using SearchService.RequestHelpers;

namespace SearchService.Endpoints;

public static class SearchEndpoints
{
    public static void MapSearchEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/search").WithTags("Search");

        group.MapGet("/", SearchItemsAsync);
    }

    private static async Task<Ok<List<Item>>> SearchItemsAsync([FromQuery] SearchParams searchParams)
    {
        var query = DB
            .Find<Item>()
            .Sort(x => x.Ascending(a => a.Make));

        if (!string.IsNullOrWhiteSpace(searchParams.SearchTerm))
        {
            query.Match(Search.Full, searchParams.SearchTerm).SortByTextScore();
        }

        query = searchParams.OrderBy switch
        {
            "make" => query.Sort(x => x.Descending(a => a.Make)),
            "new" => query.Sort(x => x.Descending(a => a.CreatedAt)),
            _ => query.Sort(x => x.Ascending(a => a.AuctionEnd))
        };
        
        query = searchParams.FilterBy switch
        {
            "active" => query.Match(x => x.AuctionEnd > DateTime.UtcNow),
            "ended" => query.Match(x => x.AuctionEnd < DateTime.UtcNow),
            "endingSoon" => query.Match(x => x.AuctionEnd < DateTime.UtcNow.AddHours(6) && x.AuctionEnd > DateTime.UtcNow),
            _ => query
        };

        if (!string.IsNullOrEmpty(searchParams.Seller))
        {
            query.Match(x => x.Seller == searchParams.Seller);
        }

        if (!string.IsNullOrEmpty(searchParams.Winner)) 
        {
            query.Match(x => x.Winner == searchParams.Winner);
        }

        query.Skip((searchParams.PageNumber - 1) * searchParams.PageSize)
            .Limit(searchParams.PageSize);

        var result = await query.ExecuteAsync();

        return TypedResults.Ok(result);
    }
}
