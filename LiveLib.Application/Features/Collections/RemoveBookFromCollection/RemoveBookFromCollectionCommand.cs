using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Models.Books;
using MediatR;

namespace LiveLib.Application.Features.Collections.RemoveBookFromCollection
{
    public record RemoveBookFromCollectionCommand(Guid BookId, Guid CollectionId) : IRequest<Result<BookDto>>;
}
