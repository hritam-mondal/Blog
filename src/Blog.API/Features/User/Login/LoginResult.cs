namespace Blog.API.Features.User.Login;

public class LoginResult
{
    public bool Success { get; set; }
    public string? Token { get; set; }
    public string? ErrorMessage { get; set; }
}
