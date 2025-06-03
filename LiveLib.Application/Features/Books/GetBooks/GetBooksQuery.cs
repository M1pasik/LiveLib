using LiveLib.Application.Models.Books;
using MediatR;

namespace LiveLib.Application.Features.Books.GetBooks
{
    public class GetBooksQuery : IRequest<ICollection<BookDto>>;
}
