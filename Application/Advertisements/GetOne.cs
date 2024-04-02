using MediatR;
using Microsoft.EntityFrameworkCore;
using NOLA_API.Application.Core;
using NOLA_API.DataModels;

namespace NOLA_API.Application.Advertisements
{
    public class GetOne
    {
        public class Query : IRequest<Result<Advertisement>>
        {
            public Guid Id { get; set; }
        }
        public class Handler : IRequestHandler<Query, Result<Advertisement>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<Advertisement>> Handle(Query request, CancellationToken cancellationToken)
            {
                var adv = await _context.Ads
                    .FirstOrDefaultAsync(x => x.Id == request.Id);
                return Result<Advertisement>.Success(adv);
            }
        }
    }
}