using Blog.API.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.API.Features.Post.List;

public sealed class ListPostHandler : IRequestHandler<ListPostQuery, ListPostResponse>
{
    private readonly ApplicationDbContext _context;

    public ListPostHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ListPostResponse> Handle(ListPostQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Entities.Post> query = _context.Posts;

        // Pagination
        int pageNumber = request.PageNumber > 0 ? request.PageNumber : 1;
        int pageSize = request.PageSize > 0 ? request.PageSize : 6;
        int skip = (pageNumber - 1) * pageSize;

        // Apply pagination
        query = query.Skip(skip).Take(pageSize);

        // Check if search query is provided
        if (!string.IsNullOrEmpty(request.SearchQuery))
        {
            // Perform search based on title or content
            query = query.Where(post =>
                post.Title.Contains(request.SearchQuery) ||
                post.Content.Contains(request.SearchQuery));
        }

        // Retrieve filtered posts from the database
        var posts = await query.ToListAsync(cancellationToken);

        // Get total count of posts
        int totalCount = await _context.Posts.CountAsync();

        return new ListPostResponse { Posts = posts, TotalCount = totalCount };
    }
}
