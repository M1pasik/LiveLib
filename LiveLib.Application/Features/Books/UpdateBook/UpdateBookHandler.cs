using AutoMapper;
using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Interfaces;
using LiveLib.Application.Models.Books;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiveLib.Application.Features.Books.UpdateBook
{
    public class UpdateBookHandler : HandlerBase, IRequestHandler<UpdateBookCommand, Result<BookDetailDto>>
    {
        public UpdateBookHandler(IMapper mapper, IDatabaseContext context) : base(mapper, context)
        {
        }

        public async Task<Result<BookDetailDto>> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.BookPublisher)
                .Include(b => b.Genre)
                .FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);

            if (book == null)
            {
                return Result<BookDetailDto>.NotFound($"Book {request.Id} not found");
            }

            _mapper.Map(request.Book, book);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(_mapper.Map<BookDetailDto>(book));
        }
    }
}
