using LiveLib.Application.Models.Authors;
using MediatR;

namespace LiveLib.Application.Features.Authors.GetAuthors
{
    public class GetAuthorsQuery : IRequest<ICollection<AuthorDto>>;
}
