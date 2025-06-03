using AutoMapper;
using AutoMapper.QueryableExtensions;
using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Interfaces;
using LiveLib.Application.Models.BookPublishers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiveLib.Application.Features.BookPublishers.GetBookPublisherById
{
    public class GetBookPublisherByIdHandler : HandlerBase, IRequestHandler<GetBookPublisherByIdQuery, Result<BookPublisherDetailDto>>
    {
        public GetBookPublisherByIdHandler(IMapper mapper, IDatabaseContext context) : base(mapper, context)
        {
        }

        public async Task<Result<BookPublisherDetailDto>> Handle(GetBookPublisherByIdQuery request, CancellationToken cancellationToken)
        {
            var bookPublisher = await _context.BookPublishers
                .Where(a => a.Id == request.Id)
                .ProjectTo<BookPublisherDetailDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            return bookPublisher is null ? Result<BookPublisherDetailDto>.NotFound($"BookPublisher {request.Id} not found") :
                Result.Success(_mapper.Map<BookPublisherDetailDto>(bookPublisher));
        }
    }
}
