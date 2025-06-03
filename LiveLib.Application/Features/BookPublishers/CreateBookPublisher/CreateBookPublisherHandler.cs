using AutoMapper;
using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Interfaces;
using LiveLib.Domain.Models;
using MediatR;

namespace LiveLib.Application.Features.BookPublishers.CreateBookPublisher
{
    public class CreateBookPublisherHandler : HandlerBase, IRequestHandler<CreateBookPublisherCommand, Result<Guid>>
    {
        public CreateBookPublisherHandler(IMapper mapper, IDatabaseContext context) : base(mapper, context)
        {
        }

        public async Task<Result<Guid>> Handle(CreateBookPublisherCommand request, CancellationToken cancellationToken)
        {
            var bookPublisher = _mapper.Map<BookPublisher>(request);
            _context.BookPublishers.Add(bookPublisher);
            var saved = await _context.SaveChangesAsync(cancellationToken);

            return saved == 0 ? Result<Guid>.ServerError("Book not saved") : Result.Success(bookPublisher.Id);
        }
    }
}
