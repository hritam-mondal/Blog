using Blog.API.Features.User.Register;
using MediatR;

namespace Blog.API.Features.Authentication.Register;

public class RegisterCommand : IRequest<RegisterResult>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? Role { get; set; } = "Author";
}
