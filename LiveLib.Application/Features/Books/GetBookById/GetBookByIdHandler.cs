using AutoMapper;
using AutoMapper.QueryableExtensions;
using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Interfaces;
using LiveLib.Application.Models.Books;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiveLib.Application.Features.Books.GetBookById
{
    public class GetBookByIdHandler : HandlerBase, IRequestHandler<GetBookByIdQuery, Result<BookDetailDto>>
    {
        public GetBookByIdHandler(IMapper mapper, IDatabaseContext context) : base(mapper, context)
        {
        }

        public async Task<Result<BookDetailDto>> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            var book = await _context.Books
                .Where(b => b.Id == request.Id)
                .ProjectTo<BookDetailDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            return book is null ? Result<BookDetailDto>.NotFound($"Book {request.Id} not found") :
                Result.Success(book);
        }
    }
}
