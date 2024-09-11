
using MediatR;
using NOLA_API.Application.Core;

namespace NOLA_API.Application.Advertisements;

public class Delete
{
    public class Command : IRequest<Result<Unit>>
    {
        public Guid Id { get; set; }
    }

    public class Handler(DataContext context) : IRequestHandler<Command, Result<Unit>>
    {
        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {

            var ad = await context.Ads.FindAsync(request.Id, cancellationToken);
            if (ad == null) return null;
            context.Ads.Remove(ad);

            var result = await context.SaveChangesAsync(cancellationToken) > 0;
            if (!result) return Result<Unit>.Failure("Failed to create activity");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}