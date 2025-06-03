using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Models.Authors;
using MediatR;

namespace LiveLib.Application.Features.Authors.GetAuthorById
{
    public record GetAuthorByIdQuery(Guid Id) : IRequest<Result<AuthorDetailDto>>;
}
