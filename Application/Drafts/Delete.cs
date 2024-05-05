
using MediatR;
using NOLA_API.Application.Core;

namespace NOLA_API.Application.Drafts
{
    public class Delete
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {

                var ad = await _context.Drafts.FindAsync(request.Id);
                if (ad == null) return null;
                _context.Drafts.Remove(ad);

                var result = await _context.SaveChangesAsync() > 0;
                if (!result) return Result<Unit>.Failure("Failed to delete draft");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}