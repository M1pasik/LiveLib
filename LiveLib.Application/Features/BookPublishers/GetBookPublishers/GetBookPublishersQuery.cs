using LiveLib.Application.Models.BookPublishers;
using MediatR;

namespace LiveLib.Application.Features.BookPublishers.GetBookPublishers
{
    public class GetBookPublishersQuery : IRequest<ICollection<BookPublisherDto>>;
}
