namespace Blog.API.Features.Post.List;

public class ListPostResponse
{
    public List<Entities.Post>? Posts { get; set; }
    public int? TotalCount { get; set; }
}