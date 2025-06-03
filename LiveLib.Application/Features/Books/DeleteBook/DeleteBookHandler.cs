using AutoMapper;
using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Interfaces;
using LiveLib.Application.Models.Books;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiveLib.Application.Features.Books.DeleteBook
{
    public class DeleteBookHandler : HandlerBase, IRequestHandler<DeleteBookCommand, Result<BookDetailDto>>
    {
        public DeleteBookHandler(IMapper mapper, IDatabaseContext context) : base(mapper, context)
        {
        }

        public async Task<Result<BookDetailDto>> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == request.Id);
            if (book == null)
            {
                return Result<BookDetailDto>.NotFound($"Book {request.Id} not found");
            }

            _context.Books.Remove(book);
            var removed = await _context.SaveChangesAsync(cancellationToken);

            return removed == 0 ? Result<BookDetailDto>.NotFound($"Book {request.Id} not deleted")
                : Result.Success(_mapper.Map<BookDetailDto>(book));
        }
    }
}
