using MediatR;

namespace Blog.API.Features.Post.List;

public class ListPostQuery : IRequest<ListPostResponse>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string? SearchQuery { get; set; }
}
