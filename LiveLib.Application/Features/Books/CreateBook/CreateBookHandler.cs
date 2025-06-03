using AutoMapper;
using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Interfaces;
using LiveLib.Domain.Models;
using MediatR;

namespace LiveLib.Application.Features.Books.CreateBook
{
    public class CreateBookHandler : HandlerBase, IRequestHandler<CreateBookCommand, Result<Guid>>
    {

        public CreateBookHandler(IMapper mapper, IDatabaseContext context) : base(mapper, context)
        {
        }

        public async Task<Result<Guid>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            var book = _mapper.Map<Book>(request);
            _context.Books.Add(book);
            var saved = await _context.SaveChangesAsync(cancellationToken);

            return saved == 0 ? Result<Guid>.ServerError("Book not saved") : Result.Success(book.Id);
        }

    }
}
