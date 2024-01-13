using Blog.API.Contracts;
using Carter;
using Mapster;
using MediatR;

namespace Blog.API.Features.Authentication.Register;

public class RegisterEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/register", async (RegisterRequest request, ISender sender) =>
        {
            var command = request.Adapt<RegisterCommand>();
            var user = await sender.Send(command);
            return Results.Ok(user);
        });
    }
}
