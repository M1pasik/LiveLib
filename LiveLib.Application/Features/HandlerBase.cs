using AutoMapper;
using LiveLib.Application.Interfaces;

namespace LiveLib.Application.Features
{
    public class HandlerBase
    {
        protected readonly IMapper _mapper;
        protected readonly IDatabaseContext _context;

        public HandlerBase(IMapper mapper, IDatabaseContext context)
        {
            _mapper = mapper;
            _context = context;
        }
    }
}
