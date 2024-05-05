using MediatR;
using NOLA_API.Application.Core;
using NOLA_API.DataModels;
using Microsoft.EntityFrameworkCore;

namespace NOLA_API.Application.Drafts
{
    public class Show
    {
        public class Query : IRequest<Result<List<Draft>>> { 
        }

        public class Handler : IRequestHandler<Query, Result<List<Draft>>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<List<Draft>>> Handle(Query request, CancellationToken cancellationToken)
            {
          
                    var drafts = await _context.Drafts
                        .ToListAsync(cancellationToken);
                    return Result<List<Draft>>.Success(drafts);
            
            }
        }
    }
}