using LiveLib.Application.Commom.ResultWrapper;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace LiveLib.Application.Features.Books.UploadCover
{
    public record UploadCoverCommand(Guid BookId, IFormFile Image) : IRequest<Result<string>>;
}
