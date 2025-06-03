using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Models.BookPublishers;
using MediatR;

namespace LiveLib.Application.Features.BookPublishers.GetBookPublisherById
{
    public record GetBookPublisherByIdQuery(Guid Id) : IRequest<Result<BookPublisherDetailDto>>;
}
