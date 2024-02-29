using Blog.API.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.API.Features.Post.List;

public sealed class ListPostHandler : IRequestHandler<ListPostQuery, List<Entities.Post>>
{
    private readonly ApplicationDbContext _context;

    public ListPostHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Entities.Post>> Handle(ListPostQuery request, CancellationToken cancellationToken)
    {
        // Retrieve all posts from the database
        var posts = await _context.Posts.ToListAsync(cancellationToken);

        return posts;
    }
}
