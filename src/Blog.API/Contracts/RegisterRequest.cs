﻿namespace Blog.API.Contracts;

public class RegisterRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? Role { get; set; } = "Author";
}
