using LiveLib.Application.Commom.ResultWrapper;
using MediatR;

namespace LiveLib.Application.Features.Books.GetCover
{
    public record GetCoverQuery(Guid BookId, string FileName) : IRequest<Result<byte[]>>;
}
