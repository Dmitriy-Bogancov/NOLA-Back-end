using MediatR;
using Microsoft.EntityFrameworkCore;
using NOLA_API.Application.Core;
using NOLA_API.DataModels;

namespace NOLA_API.Application.Drafts
{
    public class GetOne
    {
        public class Query : IRequest<Result<Draft>>
        {
            public Guid Id { get; set; }
        }
        public class Handler : IRequestHandler<Query, Result<Draft>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<Draft>> Handle(Query request, CancellationToken cancellationToken)
            {

                var draft = await _context.Drafts
                    .FirstOrDefaultAsync(x => x.Id == request.Id);
                return Result<Draft>.Success(draft);
            }
        }
    }
}