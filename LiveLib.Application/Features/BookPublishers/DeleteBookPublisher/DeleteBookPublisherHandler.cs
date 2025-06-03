using AutoMapper;
using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Interfaces;
using LiveLib.Application.Models.BookPublishers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiveLib.Application.Features.BookPublishers.DeleteBookPublisher
{
    public class DeleteBookPublisherHandler : HandlerBase, IRequestHandler<DeleteBookPublisherCommand, Result<BookPublisherDetailDto>>
    {
        public DeleteBookPublisherHandler(IMapper mapper, IDatabaseContext context) : base(mapper, context)
        {
        }

        public async Task<Result<BookPublisherDetailDto>> Handle(DeleteBookPublisherCommand request, CancellationToken cancellationToken)
        {
            var bookPublisher = await _context.BookPublishers.FirstOrDefaultAsync(b => b.Id == request.Id);
            if (bookPublisher == null)
            {
                return Result<BookPublisherDetailDto>.NotFound($"BookPublisher {request.Id} not found");
            }

            _context.BookPublishers.Remove(bookPublisher);
            var removed = await _context.SaveChangesAsync(cancellationToken);

            return removed == 0 ? Result<BookPublisherDetailDto>.ServerError($"BookPublisher {request.Id} not deleted")
                : Result.Success(_mapper.Map<BookPublisherDetailDto>(bookPublisher));
        }
    }
}
