using AutoMapper;
using AutoMapper.QueryableExtensions;
using LiveLib.Application.Interfaces;
using LiveLib.Application.Models.Books;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiveLib.Application.Features.Books.GetBooks
{
    public class GetBooksHandler : HandlerBase, IRequestHandler<GetBooksQuery, ICollection<BookDto>>
    {
        public GetBooksHandler(IMapper mapper, IDatabaseContext context) : base(mapper, context)
        {
        }

        public async Task<ICollection<BookDto>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
        {
            return await _context.Books.AsNoTracking()
                            .ProjectTo<BookDto>(_mapper.ConfigurationProvider)
                            .ToListAsync(cancellationToken);
        }
    }
}
