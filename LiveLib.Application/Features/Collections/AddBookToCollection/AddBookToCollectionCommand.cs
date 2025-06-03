using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Models.Books;
using MediatR;

namespace LiveLib.Application.Features.Collections.AddBookToCollection
{
    public record AddBookToCollectionCommand(Guid BookId, Guid CollectionId) : IRequest<Result<BookDto>>;
}
