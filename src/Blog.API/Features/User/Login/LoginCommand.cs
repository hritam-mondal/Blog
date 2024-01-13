using Blog.API.Features.User.Login;
using MediatR;

namespace Blog.API.Features.Authentication.Login;

public class LoginCommand : IRequest<LoginResult>
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}
