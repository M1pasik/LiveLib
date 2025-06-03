using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Models.BookPublishers;
using MediatR;

namespace LiveLib.Application.Features.BookPublishers.DeleteBookPublisher
{
    public record DeleteBookPublisherCommand(Guid Id) : IRequest<Result<BookPublisherDetailDto>>;
}
