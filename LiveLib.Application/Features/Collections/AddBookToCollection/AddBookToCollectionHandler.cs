using AutoMapper;
using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Interfaces;
using LiveLib.Application.Models.Books;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiveLib.Application.Features.Collections.AddBookToCollection
{
    public class AddBookToCollectionHandler : HandlerBase, IRequestHandler<AddBookToCollectionCommand, Result<BookDto>>
    {
        public AddBookToCollectionHandler(IMapper mapper, IDatabaseContext context) : base(mapper, context)
        {
        }

        public async Task<Result<BookDto>> Handle(AddBookToCollectionCommand request, CancellationToken cancellationToken)
        {
            var collection = await _context.Collections
                .Include(c => c.Books)
                .FirstOrDefaultAsync(c => c.Id == request.CollectionId, cancellationToken);

            if (collection is null)
            {
                return Result<BookDto>.NotFound($"Collection {request.CollectionId} not found");
            }

            var book = await _context.Books.FindAsync(request.BookId, cancellationToken);
            if (book is null)
            {
                return Result<BookDto>.NotFound($"Book {request.BookId} not found");
            }

            if (collection.Books.Contains(book))
            {
                return Result<BookDto>.Conflict($"Collection {request.CollectionId} already contain book {request.BookId}");
            }

            collection.Books.Add(book);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<BookDto>.Success(_mapper.Map<BookDto>(book));
        }
    }
}
