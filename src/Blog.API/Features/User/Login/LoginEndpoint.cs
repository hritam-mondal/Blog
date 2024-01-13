using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity.Data;

namespace Blog.API.Features.Authentication.Login;

public class LoginEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/login", async (LoginRequest request, ISender sender) =>
        {
            var command = request.Adapt<LoginCommand>();
            var loginResult = await sender.Send(command);
            return Results.Ok(loginResult);
        });
    }
}
