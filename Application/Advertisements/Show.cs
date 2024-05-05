using MediatR;
using NOLA_API.Application.Core;
using NOLA_API.DataModels;
using Microsoft.EntityFrameworkCore;

namespace NOLA_API.Application.Advertisements
{
    public class Show
    {
        public class Query : IRequest<Result<List<Advertisement>>>
        {
        }

        public class Handler : IRequestHandler<Query, Result<List<Advertisement>>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<List<Advertisement>>> Handle(Query request, CancellationToken cancellationToken)
            {

                var ads = await _context.Ads
                    .ToListAsync(cancellationToken);
                return Result<List<Advertisement>>.Success(ads);
            }
        }
    }
}