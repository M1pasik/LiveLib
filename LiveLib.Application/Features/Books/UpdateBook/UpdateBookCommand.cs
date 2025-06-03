using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Models.Books;
using MediatR;

namespace LiveLib.Application.Features.Books.UpdateBook
{
    public record UpdateBookCommand(Guid Id, UpdateBookDto Book) : IRequest<Result<BookDetailDto>>;
}
