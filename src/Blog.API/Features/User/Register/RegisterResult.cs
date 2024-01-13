namespace Blog.API.Features.User.Register;

public class RegisterResult
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public Entities.User? RegisteredUser { get; set; }
}
