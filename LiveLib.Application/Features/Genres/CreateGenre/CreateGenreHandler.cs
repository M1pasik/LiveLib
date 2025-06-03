using AutoMapper;
using LiveLib.Application.Commom.ResultWrapper;
using LiveLib.Application.Interfaces;
using LiveLib.Domain.Models;
using MediatR;

namespace LiveLib.Application.Features.Genres.CreateGenre
{
    public class CreateGenreHandler : HandlerBase, IRequestHandler<CreateGenreCommand, Result<Guid>>
    {
        public CreateGenreHandler(IMapper mapper, IDatabaseContext context) : base(mapper, context)
        {

        }

        public async Task<Result<Guid>> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
        {
            var genre = _mapper.Map<Genre>(request);
            _context.Genres.Add(genre);
            var saved = await _context.SaveChangesAsync(cancellationToken);

            return saved == 0 ? Result<Guid>.ServerError("Genre not saved") : Result.Success(genre.Id);
        }
    }
}
