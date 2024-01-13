using Blog.API.Contracts;
using Carter;
using Mapster;
using MediatR;

namespace Blog.API.Features.Post.Create;

public class CreatePostEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/post", async (CreatePostRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreatePostCommand>();
            var loginResult = await sender.Send(command);
            return Results.Ok(loginResult);
        });
    }
}
