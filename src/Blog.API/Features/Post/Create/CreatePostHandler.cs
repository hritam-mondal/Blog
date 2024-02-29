using Blog.API.Infrastructure.Data;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace Blog.API.Features.Post.Create;

public sealed class CreatePostHandler : IRequestHandler<CreatePostCommand, CreatePostResult>
{
    private readonly IValidator<CreatePostCommand> _validator;
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreatePostHandler(ApplicationDbContext context, IValidator<CreatePostCommand> validator, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _validator = validator;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<CreatePostResult> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate the command using the validator
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            // Retrieve the authorization token from the request headers
            string? token = _httpContextAccessor.HttpContext!.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            Guid authorId = Guid.Empty;
            if (token != null)
            {
                authorId = Guid.Parse(GetUserIdFromToken(token)!);
            }
            if (!validationResult.IsValid)
            {
                // Handle validation errors (e.g., return error response)
                return new CreatePostResult { Success = false, ErrorCode = "ValidationFailed", CreatedPost = null };
            }

            // Check if the author exists
            var author = await _context.Users.FirstOrDefaultAsync(u => u.Id == authorId, cancellationToken);
            if (author == null)
            {
                // Handle the case where the author is not found
                return new CreatePostResult { Success = false, ErrorCode = "AuthorNotFound", CreatedPost = null };
            }

            // Create a new blog post entity
            var blogPost = new Entities.Post
            {
                Id = Guid.NewGuid(),
                Title = request.Title!,
                Content = request.Content!,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Author = $"{author.FirstName} {author.LastName}"
            };

            // Save the blog post to the database
            await _context.Posts.AddAsync(blogPost, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            // Return the result with the created post
            return new CreatePostResult { Success = true, ErrorCode = null, CreatedPost = blogPost };
        }
        catch (ValidationException ex)
        {
            // Log validation errors
            // Handle validation exception (e.g., return error response)
            Console.WriteLine($"Validation failed: {ex.Message}");
            return new CreatePostResult { Success = false, ErrorCode = "ValidationFailed", CreatedPost = null };
        }
        catch (DbUpdateException ex)
        {
            // Log database update errors
            // Handle database update exception (e.g., return error response)
            Console.WriteLine($"Database update error: {ex.Message}");
            return new CreatePostResult { Success = false, ErrorCode = "DatabaseError", CreatedPost = null };
        }
        catch (Exception ex)
        {
            // Log other exceptions
            // Handle other exceptions (e.g., return error response)
            Console.WriteLine($"An error occurred: {ex.Message}");
            return new CreatePostResult { Success = false, ErrorCode = "UnknownError", CreatedPost = null };
        }
    }

    private string? GetUserIdFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

        // Get the "UserId" claim from the token's claims
        var userIdClaim = jwtToken?.Claims.FirstOrDefault(claim => claim.Type == "UserId");

        return userIdClaim?.Value;
    }
}