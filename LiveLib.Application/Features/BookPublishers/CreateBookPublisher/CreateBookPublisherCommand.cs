
using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Common.Mapping;
using LiveLib.Domain.Models;
using MediatR;

namespace LiveLib.Application.Features.BookPublishers.CreateBookPublisher
{
    public class CreateBookPublisherCommand : IRequest<Result<Guid>>, IMapWith<BookPublisher>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
