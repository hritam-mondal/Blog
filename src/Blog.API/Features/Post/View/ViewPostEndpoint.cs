using Carter;
using MediatR;

namespace Blog.API.Features.Post.View;

public class ViewPostEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/posts/{postId}", async (ISender sender, string postId) =>
        {
            var post = await sender.Send(new ViewPostQuery(postId));

            if (post == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(post);
        });
    }
}
