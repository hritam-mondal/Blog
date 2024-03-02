using Blog.API.Infrastructure.Data;
using MediatR;

namespace Blog.API.Features.Post.View;

public sealed class ViewPostHandler : IRequestHandler<ViewPostQuery, Entities.Post>
{
    private readonly ApplicationDbContext _context;

    public ViewPostHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Entities.Post> Handle(ViewPostQuery request, CancellationToken cancellationToken)
    {
        // Retrieve the post from the database based on the postId in the request
        var post = await _context.Posts.FindAsync(request.PostId, cancellationToken);

        // If the post is not found, return null
        if (post == null)
        {
            throw new Exception("Post not found");
        }

        return post;
    }
}
