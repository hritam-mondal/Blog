using MediatR;

namespace Blog.API.Features.Post.View;

public class ViewPostQuery(string postId) : IRequest<Entities.Post>
{
    public string PostId { get; } = postId;
}
