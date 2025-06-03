using AutoMapper;
using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Interfaces;
using MediatR;

namespace LiveLib.Application.Features.Books.GetCover
{
    public class GetCoverHandler : HandlerBase, IRequestHandler<GetCoverQuery, Result<byte[]>>
    {
        private readonly ICacheProvider _cacheProvider;

        public GetCoverHandler(IMapper mapper, IDatabaseContext context, ICacheProvider cacheProvider) : base(mapper, context)
        {
            _cacheProvider = cacheProvider;
        }

        public async Task<Result<byte[]>> Handle(GetCoverQuery request, CancellationToken cancellationToken)
        {
            var imageBytes = await _cacheProvider.BytesGetAsync($"book:{request.BookId}:image:{request.FileName}", cancellationToken);

            return imageBytes is null ? Result<byte[]>.NotFound($"Book cover {request.FileName} not found") :
                Result<byte[]>.Success(imageBytes);
        }
    }
}
