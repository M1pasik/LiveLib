using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Models.BookPublishers;
using MediatR;

namespace LiveLib.Application.Features.BookPublishers.UpdateBookPublisher
{
    public record UpdateBookPublisherCommand(Guid Id, UpdateBookPublisherDto BookPublisher) : IRequest<Result<BookPublisherDetailDto>>;
}
