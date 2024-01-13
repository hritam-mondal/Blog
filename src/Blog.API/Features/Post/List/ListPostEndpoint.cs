using Carter;
using MediatR;

namespace Blog.API.Features.Post.List;

public class ListPostEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/posts", async (ISender sender) =>
        {
            var posts = await sender.Send(new ListPostQuery());
            return Results.Ok(posts);
        });
    }
}
