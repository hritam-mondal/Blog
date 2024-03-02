using Carter;
using MediatR;

namespace Blog.API.Features.Post.List;

public class ListPostEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/posts", async (ISender sender, HttpRequest request) =>
        {
            var searchQuery = request.Query["search"].ToString();
            var pageNumber = Convert.ToInt32(request.Query["pageNumber"]);
            var pageSize = Convert.ToInt32(request.Query["pageSize"]);

            // Send query with search parameters
            var posts = await sender.Send(new ListPostQuery { SearchQuery = searchQuery, PageNumber = pageNumber, PageSize = pageSize });
            return Results.Ok(posts);
        });
    }
}
