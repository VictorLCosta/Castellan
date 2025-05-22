using Microsoft.AspNetCore.Http.HttpResults;
using SearchService.Entities;

namespace SearchService.Endpoints;

public static class SearchEndpoints
{
    public static void MapSearchEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/search").WithTags("Search");

        group.MapGet("/", SearchItemsAsync);
    }

    private static async Task<Ok<List<Item>>> SearchItemsAsync(string searchTerm)
    {
        var query = DB
            .Find<Item>()
            .Sort(x => x.Ascending(a => a.Make));

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query.Match(Search.Full, searchTerm).SortByTextScore();
        }

        var result = await query.ExecuteAsync();

        return TypedResults.Ok(result);
    }
}
