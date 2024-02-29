using MediatR;

namespace Blog.API.Features.Post.Create;

public class CreatePostCommand : IRequest<CreatePostResult>
{
    public string? Title { get; set; }
    public string? Content { get; set; }
}
