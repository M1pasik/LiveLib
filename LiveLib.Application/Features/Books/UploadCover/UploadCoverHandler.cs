using AutoMapper;
using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiveLib.Application.Features.Books.UploadCover
{
    public class UploadCoverHandler : HandlerBase, IRequestHandler<UploadCoverCommand, Result<string>>

    {
        private readonly ICacheProvider _cache;

        public UploadCoverHandler(IMapper mapper, IDatabaseContext context, ICacheProvider cacheProvider) : base(mapper, context)
        {
            _cache = cacheProvider;
        }

        public async Task<Result<string>> Handle(UploadCoverCommand request, CancellationToken cancellationToken)
        {
            if (request.Image is null || request.Image.Length == 0)
            {
                return Result<string>.BadRequest($"Image not found");
            }

            var book = await _context.Books
             .FirstOrDefaultAsync(g => g.Id == request.BookId, cancellationToken);

            if (book is null)
            {
                return Result<string>.NotFound($"Book {request.BookId} not found");
            }

            byte[] imageBytes;
            using (var ms = new MemoryStream())
            {
                await request.Image.CopyToAsync(ms, cancellationToken);
                imageBytes = ms.ToArray();
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(request.Image.FileName);
            await _cache.BytesSetAsync($"book:{book.Id}:image:{fileName}", imageBytes, cancellationToken);

            book.CoverId = fileName;
            await _context.SaveChangesAsync(cancellationToken);

            return Result<string>.Success(book.CoverId);
        }
    }
}
