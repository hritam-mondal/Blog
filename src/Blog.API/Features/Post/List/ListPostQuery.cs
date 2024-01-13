using MediatR;

namespace Blog.API.Features.Post.List;

public class ListPostQuery : IRequest<List<Entities.Post>> { }
